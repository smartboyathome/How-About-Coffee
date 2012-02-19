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
                Button1.Content = "Udupi Café";
                Button2.Content = "Kitanda";
                Button3.Content = "Victor's Celtic Coffee Co";
                AvailableTimesScreen = false;
            }
            else
            {
                
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Button1.Content = "Monday Feb 20 at 3:00 pm";
            Button2.Content = "Monday Feb 20 at 6:30 pm";
            Button3.Content = "Tuesday Feb 21 at 11:30 am";
        }
    }
}
