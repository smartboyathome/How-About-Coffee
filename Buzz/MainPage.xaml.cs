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

using Microsoft.Phone.UserData;

namespace Buzz
{
    public partial class MainPage : PhoneApplicationPage
    {
        FilterKind contactFilterKind = FilterKind.None;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            //name.IsChecked = true;
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
        }

        /*
        private void FilterChange(object sender, RoutedEventArgs e)
        {
            String option = ((RadioButton)sender).Name;

            InputScope scope = new InputScope();
            InputScopeName scopeName = new InputScopeName();

            switch (option)
            {
                case "name":
                    contactFilterKind = FilterKind.DisplayName;
                    scopeName.NameValue = InputScopeNameValue.Text;
                    break;

                case "phone":
                    contactFilterKind = FilterKind.PhoneNumber;
                    scopeName.NameValue = InputScopeNameValue.TelephoneNumber;
                    break;

                case "email":
                    contactFilterKind = FilterKind.EmailAddress;
                    scopeName.NameValue = InputScopeNameValue.EmailSmtpAddress;
                    break;

                default:
                    contactFilterKind = FilterKind.None;
                    break;
            }

            scope.Names.Add(scopeName);
            contactFilterString.InputScope = scope;
            contactFilterString.Focus();
        }
        */

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
            //MessageBox.Show(e.State.ToString());

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

            //NavigationService.Navigate(new Uri("/ContactDetails.xaml", UriKind.Relative));

            List<FreeSlot> FreeList = new List<FreeSlot>();

            FreeList.Add(new FreeSlot(DateTime.UtcNow, DateTime.UtcNow));

            ServiceInterface.SendFreeList("2067130182", "2067131688", FreeList);
        }

        private void contactFilterString_TextChanged(object sender, TextChangedEventArgs e)
        {
            DoSearch();
        }

        private void Bigbutton_Click(object sender, RoutedEventArgs e)
        {
            List<FreeSlot> FreeList = new List<FreeSlot>();
            
            FreeList.Add(new FreeSlot(DateTime.UtcNow, DateTime.UtcNow));

            ServiceInterface.SendFreeList("2067130182", "2067131688", FreeList);
        }
    }
}