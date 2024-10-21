using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UserRegistry.Models;


namespace UserRegistry.Views
{
    public sealed partial class ChiamateHTTP : Page
    {
        HttpClient httpCLient = new HttpClient();
        public ChiamateHTTP()
        {
            this.InitializeComponent();
        }

        private async void BtnHttpGetComments_Click(object sender, RoutedEventArgs e)
        {
            CommentsLoadingRing.IsActive= true;
            var comments = await httpCLient.GetFromJsonAsync<List<Comment>>("https://jsonplaceholder.typicode.com/comments");
            CommentListView.ItemsSource = comments;
            CommentsLoadingRing.IsActive = false;
        }
    }
}
