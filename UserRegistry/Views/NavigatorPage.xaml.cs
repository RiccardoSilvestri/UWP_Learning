using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace UserRegistry.Views
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class NavigatorPage : Page
    {
        public NavigatorPage()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MaximizeWindow();
        }

        private void MaximizeWindow()
        {
            var view = ApplicationView.GetForCurrentView();

            //view.TryEnterFullScreenMode();

            view.TryResizeView(new Size(view.VisibleBounds.Width, view.VisibleBounds.Height));
        }
        private void NavigateToHome_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Home));
        }

        private void NavigateToLogout_Click(object sender, RoutedEventArgs e)
        {
          Frame.Navigate(typeof(Login));
        }

        private void NavigateToUsers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Register));
        }
        private void NavitageToHTTPCall_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(ChiamateHTTP));
        }


        private void CloseAndExitApplication_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
