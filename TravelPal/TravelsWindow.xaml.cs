using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Classes;
using TravelPal.Manager;

namespace TravelPal
{
    public partial class TravelsWindow : Window
    {
        public TravelsWindow()
        {
            InitializeComponent();

            UpdateUi();
        }

        public void UpdateUi()
        {
            // FILL IN CODE
            IUser signedInUser = UserManager.signedInUser!;
            User user = (User)signedInUser;
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            lblWelcomeUsername.Content = "Welcome " + textInfo.ToTitleCase(signedInUser.Username);
            lblFullName.Content = signedInUser.FullName;

            lstMyTravels.Items.Clear();
            if (user.Travels != null)
            {
                btnDetails.IsEnabled = true;
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
            else
            {
                btnDetails.IsEnabled = false;
            }

        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
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

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)lstMyTravels.SelectedItem;

            TravelDetailsWindow detailsWindow = new TravelDetailsWindow(selectedItem);
            detailsWindow.Show();
            Close();
        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new AddTravelWindow();
            addTravelWindow.Show();
            Close();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            // FILL IN CODE

        }
    }
}
