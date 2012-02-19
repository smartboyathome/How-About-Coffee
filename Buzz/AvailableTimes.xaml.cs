using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Buzz
{
    public partial class AvailableTimes : PhoneApplicationPage
    {
        bool AvailableTimesScreen = true;

        public AvailableTimes()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableTimesScreen)
            {
                txtHeading.Text = "Where to meet?";

                Button1.Content = ""; // "Udupi Café";
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri("/upudi.png", UriKind.Relative));
                Button1.Background = ib;

                Button2.Content = ""; // "Kitanda";
                ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri("/kitanda.png", UriKind.Relative));
                Button2.Background = ib;

                Button3.Content = ""; // "Victor's Celtic Coffee Co";
                ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri("/VICTOR.png", UriKind.Relative));
                Button3.Background = ib;

                AvailableTimesScreen = false;
            }
            else
            {
                
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            txtHeading.Text = "When to meet?";
            Button1.Content = "Monday Feb 20\n3:00 pm";
            Button2.Content = "Monday Feb 20\n6:30 pm";
            Button3.Content = "Tuesday Feb 21\n11:30 am";

            //MessageBox.Show("Height: " + Button1.ActualHeight.ToString() + "\nWidth: " + Button1.ActualWidth.ToString());
        }
    }
}
