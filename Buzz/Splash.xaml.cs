using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Buzz
{
    public partial class Splash : PhoneApplicationPage
    {
        // Constructor
        public Splash()
        {
            InitializeComponent();
        }

        private void homePage_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();

            webBrowserTask.Uri = new Uri("http://www.howboutcoffeeapp.com", UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void Splash_MediaEnded(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void ContentPanel_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}