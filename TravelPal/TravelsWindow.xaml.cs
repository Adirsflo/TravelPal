using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Classes;
using TravelPal.Manager;

namespace TravelPal
{
    public partial class TravelsWindow : Window
    {
        IUser signedInUser = UserManager.signedInUser!;
        public TravelsWindow()
        {
            InitializeComponent();

            if (signedInUser.GetType().ToString() == (typeof(User).ToString()))
            {
                // If User is Signed In... Load User UI
                UpdateUserUi();
            }
            else if (signedInUser.GetType().ToString() == (typeof(Admin).ToString()))
            {
                // If Admin is Signed In... Load Admin UI
                UpdateAdminUi();
            }
        }

        private void UpdateAdminUi() // Loading Admin UI
        {
            Admin admin = (Admin)signedInUser;
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            // Capitalizing the first letter in username
            lblWelcomeUsername.Content = "Welcome " + textInfo.ToTitleCase(signedInUser.Username);
            lblFullName.Content = signedInUser.FullName;

            lblMyTravels.Content = "All Travels";

            btnAddTravel.Visibility = Visibility.Hidden;
            lblAllUsers.Visibility = Visibility.Visible;
            cbAllUsers.Visibility = Visibility.Visible;

            foreach (var user in UserManager.UserList)
            {
                // Loading and capitalizing all first letter of Username (Except admin)
                if (user.GetType().ToString() != (typeof(Admin).ToString()))
                {
                    cbAllUsers.Items.Add(textInfo.ToTitleCase(user.Username));
                }
            }
        }

        private void UpdateUserUi() // Loading User UI
        {
            User user = (User)signedInUser;

            // Capitalixing first name of Username
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            lblWelcomeUsername.Content = "Welcome " + textInfo.ToTitleCase(signedInUser.Username);
            lblFullName.Content = signedInUser.FullName;

            lstMyTravels.Items.Clear();

            btnDetails.IsEnabled = true;
            foreach (var travel in user.Travels)
            {
                ListViewItem item = new();

                // Setting the right format in the ListView
                item.Content = new
                {
                    Trip = travel.Destination,
                    Period = travel.StartDate.ToString("yyyy-MM-dd") + " - " + travel.EndDate.ToString("yyyy-MM-dd"),
                    TravelPals = travel.Travellers,
                };
                item.Tag = travel;

                lstMyTravels.Items.Add(item);
            }
        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e) // Allows user to sign out and making sure 
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to sign out?", "Signing out", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                UserManager.signedInUser = null;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e) // Viewing selected trips details in a new window
        {
            ListBoxItem selectedItem = (ListBoxItem)lstMyTravels.SelectedItem;

            if (selectedItem != null)
            {
                TravelDetailsWindow detailsWindow = new TravelDetailsWindow(selectedItem);
                detailsWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Please select a trip to show!", "Warning");
            }
        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e) // Opening a new window for adding a new travel
        {
            AddTravelWindow addTravelWindow = new AddTravelWindow();
            addTravelWindow.Show();
            Close();
        }

        private void blkInformation_Click(object sender, RoutedEventArgs e) // Viewing information for current window
        {
            if (signedInUser.GetType().ToString() == (typeof(User).ToString()))
            {
                // Information for User
                MessageBox.Show("Welcome to TravelPal!\n\n" +
                    "-You can view your trip under \"My Travels\"\n" +
                    "-To view a certain trip, select the trip and then click on \"Details\" button\n" +
                    "-If you wish to add a new trip, click on \"Add Travel\" button\n" +
                    "-To remove a trip from your travels, select the trip and then click on \"Remove\" button\n" +
                    "-On your upper right corner, you can choose to view User-profile, or click on \"Signing Out\" button", "Information - Navigation");
            }
            else if (signedInUser.GetType().ToString() == (typeof(Admin).ToString()))
            {
                // Information for Admin
                MessageBox.Show("Welcome to Admin-view!\n\n" +
                    "-You can view all trips under \"All Travels\"\n" +
                    "-To view the trips from a user, select the trip under \"Users\"\n" +
                    "-If you wish to further view a certain trip from a user, select a trip and click on \"Details\" button\n" +
                    "-To remove a trip from a users travels, select the trip and then click on \"Remove\" button\n" +
                    "-On your upper right corner, you can choose to view User-profile, or click on \"Signing Out\" button", "Information - Admin Navigation");
            }
        }

        private void blkUser_Click(object sender, RoutedEventArgs e) // Hyperlink for an undeveloped nice-to-have window
        {
            MessageBox.Show("This function is currently not available!", "Error");
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e) // Removes a trip
        {
            ListBoxItem selectedItem = (ListBoxItem)lstMyTravels.SelectedItem;
            ListViewItem itemToRemove = (ListViewItem)selectedItem;

            if (selectedItem != null)
            {
                Travel selectedTrip = (Travel)selectedItem.Tag;
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove the trip to {selectedTrip.Destination}?", "Remove trip", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    lstMyTravels.Items.Remove(itemToRemove);
                    MessageBox.Show($"{selectedTrip.Destination} removed successfully!", "Trip Removed");

                    try
                    {
                        ((User)UserManager.signedInUser).Travels.Remove(selectedTrip);
                        UpdateUserUi();
                    }
                    catch (InvalidCastException)
                    {
                        string selectedUser = (string)cbAllUsers.SelectedItem;
                        User user = GetUserByUsername(selectedUser);

                        user.Travels.Remove(selectedTrip);
                        lstMyTravels.Items.Remove(selectedTrip);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a trip to remove!", "Warning");
            }
        }

        private void cbAllUsers_SelectionChanged(object sender, SelectionChangedEventArgs e) // ComboBox for Admin that shows All Users
        {
            lstMyTravels.Items.Clear();
            string selectedUser = (string)cbAllUsers.SelectedItem;
            User user = GetUserByUsername(selectedUser);

            if (user != null)
            {
                lblMyTravels.Content = $"{selectedUser}s Travels";

                foreach (var travel in user.Travels)
                {
                    ListViewItem item = new();

                    item.Content = new
                    {
                        Trip = travel.Destination,
                        Period = travel.StartDate.ToString("yyyy-MM-dd") + " - " + travel.EndDate.ToString("yyyy-MM-dd"),
                        TravelPals = travel.Travellers,
                    };
                    item.Tag = travel;

                    lstMyTravels.Items.Add(item);
                }
            }
        }

        private User GetUserByUsername(string username) // Retrieving selected username
        {
            foreach (var account in UserManager.UserList)
            {
                if (account.GetType().ToString() != (typeof(Admin).ToString()) && account.Username == username.ToLower())
                {
                    User user = (User)account;
                    return user;
                }
            }
            return null;
        }
    }
}
