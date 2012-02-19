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
using System.Windows.Threading;
using Microsoft.Phone.UserData;

namespace Buzz
{
    public partial class MainPage : PhoneApplicationPage
    {
        FilterKind contactFilterKind = FilterKind.None;
        DispatcherTimer CheckInTimer = new DispatcherTimer();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            InputScope scope = new InputScope();
            InputScopeName scopeName = new InputScopeName();

            contactFilterKind = FilterKind.DisplayName;
            scopeName.NameValue = InputScopeNameValue.Text;

            scope.Names.Add(scopeName);
            contactFilterString.InputScope = scope;
            contactFilterString.Focus();

            DoSearch();
            
            CheckInTimer.Interval = new TimeSpan(0, 0, 5);
            CheckInTimer.Tick += TimerTick;
        }

        private void DoSearch()
        {
            ContactResultsLabel.Text = "results are loading...";
            ContactResultsData.DataContext = null;

            Microsoft.Phone.UserData.Contacts cons = new Microsoft.Phone.UserData.Contacts();

            cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);

            cons.SearchAsync(contactFilterString.Text, contactFilterKind, "Contacts Test #1");
        }

        private void SearchContacts_Click(object sender, RoutedEventArgs e)
        {
            DoSearch();
        }

        void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            try
            {
                //Bind the results to the list box that displays them in the UI
                ContactResultsData.DataContext = e.Results;
            }
            catch (System.Exception)
            {
                //That's okay, no results
            }

            if (ContactResultsData.Items.Count > 0)
            {
                ContactResultsLabel.Text = "results (tap name for details...)";
            }
            else
            {
                ContactResultsLabel.Text = "no results";
            }
        }

        private void ContactResultsData_Tap(object sender, GestureEventArgs e)
        {
            App.con = ((sender as ListBox).SelectedValue as Contact);
            AppointmentManager am = new AppointmentManager();

            am.FreeTimesAvailable += SendToServer;
            am.GetFreeTime();
        }

        private void SendToServer(List<Slot> list)
        {
            if (list.Count > 0)
            {
                List<FreeSlot> FreeList = new List<FreeSlot>();

                foreach (Slot s in list)
                {
                    System.Diagnostics.Debug.WriteLine("Start {0}   End {1}", s.Start.Ticks / 100000000, s.End.Ticks / 100000000);
                    FreeList.Add(new FreeSlot(s));
                }

                ServiceInterface.ResponseCallback += ResponseCallback;

                //ServiceInterface.SendFreeList("2067130182", "2067131688", FreeList);  //1
                ServiceInterface.SendFreeList("2067131688", "2067130182", FreeList);  //2
            }
        }

        void ResponseCallback(List<FreeSlot> MatchedList)
        {
            AvailableTimes at = new AvailableTimes();
            NavigationService.Navigate(new Uri("/AvailableTimes.xaml"));
        }

        void TimerTick(Object sender, EventArgs e)
        {
            ServiceInterface.CheckForResults("2067130182", "2067131688");
        }

        private void contactFilterString_TextChanged(object sender, TextChangedEventArgs e)
        {
            DoSearch();
        }

        private void Bigbutton_Click(object sender, RoutedEventArgs e)
        {
            List<Slot> FreeList = new List<Slot>();

            FreeList.Add(new Slot(new DateTime(2012, 2, 19, 12, 30, 0), new DateTime(2012, 2, 19, 16, 20, 0)));
            FreeList.Add(new Slot(new DateTime(2012, 2, 20, 11, 5, 0), new DateTime(2012, 2, 20, 13, 2, 0)));

            SendToServer(FreeList);
        }
    }
}