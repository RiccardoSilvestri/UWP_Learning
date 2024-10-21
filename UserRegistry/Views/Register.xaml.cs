using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UserRegistry.ViewModels;
using System.Linq;
using User = UserRegistry.Models.User;
using System.Diagnostics;
using System;
using Windows.UI.Xaml.Navigation;
using UserRegistry.BLogic;
using System.Collections.Generic;
using UserRegistry.Models;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UserRegistry.Views
{
    public sealed partial class Register
    {
        private readonly UserViewModel _viewModel = new();
        public Register()
        {
            InitializeComponent();
            ImportList();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Welcome.Text = "Welcome " + e.Parameter as string;
        }


        private void AddUser(object sender, RoutedEventArgs e)
        {
            _viewModel.Users.Add(_viewModel.GetUser);
            _viewModel.GetUser = new User();
            Debug.WriteLine($"Admin added User [{_viewModel.Name}] at: " + DateTime.Now);
            Console.WriteLine($"Admin added User [{_viewModel.Name}] at: " + DateTime.Now);
        }

        private void CheckInput(object sender, TextChangedEventArgs e)
        {
            BtnAddUser.IsEnabled = !string.IsNullOrEmpty(Username.Text) && !string.IsNullOrEmpty(UserSurname.Text);
        }


        private void CheckNumericInput(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            var newText = new string(textBox.Text.Where(char.IsDigit).ToArray());

            if (textBox.Text != newText)
            {
                textBox.Text = newText;
                textBox.SelectionStart = newText.Length;
            }

            if (textBox.Text.Length > textBox.MaxLength)
            {
                textBox.Text = textBox.Text.Substring(0, textBox.MaxLength);
                textBox.SelectionStart = textBox.MaxLength;
            }
        }

        private void UsernameAndSurname_PreviewKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (!char.IsLetter((char)e.Key) && e.Key != Windows.System.VirtualKey.Tab
                && e.Key != Windows.System.VirtualKey.Back && e.Key != Windows.System.VirtualKey.Cancel) 
            {
                e.Handled = true;
            }
        }

        private async void BtnExportList_Click(object sender, RoutedEventArgs e)
        {
            await FileManager.WriteToJsonFile(_viewModel.Users.ToList(), "Users.json", true);
        }

        private async void ImportList()
        {
            _viewModel.Users = new ObservableCollection<User>(await FileManager.ReadJsonFile<User>("Users.json"));
            DataContext = _viewModel;
        }

        private void BtnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.UpdateSelectedUser();
            _viewModel.GetUser = new User();
            BtnUpdateUser.IsEnabled = false;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnUpdateUser.IsEnabled = true;
        }
    }
}