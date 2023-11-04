using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Classes;
using TravelPal.Enums;
using TravelPal.Manager;

namespace TravelPal
{
    public partial class AddTravelWindow : Window
    {
        public List<IPackingListItem> TemporarilyPackingList { get; set; } = new();
        IUser signedInUser = UserManager.signedInUser;
        public AddTravelWindow()
        {
            InitializeComponent();
            UpdateUi();
        }

        private void UpdateUi() // Uppdates UI
        {
            cbNewCountry.ItemsSource = Enum.GetValues(typeof(Countries));
            cbNewCountry.SelectedIndex = -1;

            // User can only pick from todays date
            dpAddFromTravelDate.DisplayDateStart = DateTime.Now;
            dpAddToTravelDate.DisplayDateStart = DateTime.Now;

            // Displays the durration of trip
            CalculateAndDisplayDifference();

            cbNewTypeTrip.Items.Add("--Type of Trip--");
            cbNewTypeTrip.Items.Add("Vacation");
            cbNewTypeTrip.Items.Add("Work Trip");
            cbNewTypeTrip.SelectedIndex = 0;

            ClearPackinglistInput();
        }
        private void blkInformation_Click(object sender, RoutedEventArgs e) // Shows information of current window for User
        {
            MessageBox.Show("Welcome to Add Travel-Window!\n\n" +
                "-Fill in the information about your trip below \n" +
                "-If you wish to add an item to your packlist, you can do that by clicking on \"Add\" button\n" +
                "-If you wish to remove an item from your packlist, select the item and click on \"Remove\" button\n" +
                "-When you are done filling in all the information about the trip, click on \"Add Travel\" button\n" +
                "-On your upper right corner, you can choose to view User-profile, or click on \"Back\" to return", "Information - Navigation");
        }
        private void blkUser_Click(object sender, RoutedEventArgs e) // Currently not available
        {
            MessageBox.Show("This function is currently not available!", "Error");
        }
        private void ClearPackinglistInput() // Clears input fields for Packinglist
        {
            txtAddItem.Text = string.Empty;
            txtAddQuantity.Text = string.Empty;
            rbTravelDocumentFalse.IsChecked = true;
        }
        private void CalculateAndDisplayDifference() // Calculates and displays the durration of trip
        {
            // Refreshes the total days
            if (dpAddToTravelDate.SelectedDate != null && dpAddFromTravelDate.SelectedDate != null)
            {
                DateTime startDate = dpAddFromTravelDate.SelectedDate.Value;
                DateTime endDate = dpAddToTravelDate.SelectedDate.Value;
                int daysDifference = (int)(endDate - startDate).TotalDays;

                txtNewLengthOfTrip.Text = daysDifference.ToString();
            }
            else
            {
                txtNewLengthOfTrip.Text = "0";
            }
        }

        private void dpAddToTravelDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e) // Uppdates the durration of trip
        {
            CalculateAndDisplayDifference();
        }
        private void dpAddFromTravelDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e) // Uppdates the durration of trip
        {
            CalculateAndDisplayDifference();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e) // Returns back to previous window
        {
            if (txtNewCity.Text != "" || txtNewAmmountTravelers.Text != "" || txtNewMeetingDetails.Text != "" || lstAddedPacklist.Items.Count != 0 || txtAddItem.Text != "" || txtAddQuantity.Text != "")
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel?\n\nYour progress will not be saved!", "Warning", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    TravelsWindow travelsWindow = new TravelsWindow();
                    travelsWindow.Show();
                    Close();
                }
            }
            else
            {
                TravelsWindow travelsWindow = new TravelsWindow();
                travelsWindow.Show();
                Close();
            }
        }

        private void cbNewTypeTrip_SelectionChanged(object sender, SelectionChangedEventArgs e) // Views the right TextBoxes for the right trip type
        {
            if (cbNewTypeTrip.SelectedIndex == 1) // If Vacation is selected
            {
                txtNewMeetingDetails.Text = "";
                lblNewPurposeOfTrip.Visibility = Visibility.Visible;
                lblNewPurposeOfTrip.Content = "All Inclusive?";
                rbIsAllInclusive.Visibility = Visibility.Visible;
                txtNewMeetingDetails.Visibility = Visibility.Hidden;
            }
            else if (cbNewTypeTrip.SelectedIndex == 2) // If Work Trip is selected
            {
                lblNewPurposeOfTrip.Content = "Meeting Details";
                lblNewPurposeOfTrip.Visibility = Visibility.Visible;
                txtNewMeetingDetails.Visibility = Visibility.Visible;
                rbIsAllInclusive.Visibility = Visibility.Hidden;
            }
            else // If nothing is selected... Hides everything
            {
                txtNewMeetingDetails.Text = "";
                lblNewPurposeOfTrip.Visibility = Visibility.Hidden;
                rbIsAllInclusive.Visibility = Visibility.Hidden;
                txtNewMeetingDetails.Visibility = Visibility.Hidden;
            }
        }

        private void rbTravelDocumentTrue_Checked(object sender, RoutedEventArgs e) // If TravelDocument checked Yes... displays if the traveldocument IsRequired
        {
            rbIsRequired.Visibility = Visibility.Visible;
            txtAddQuantity.Visibility = Visibility.Hidden;
            lblAddQuantityOrRequired.Content = "Required?";
        }

        private void rbTravelDocumentFalse_Checked(object sender, RoutedEventArgs e) // If TravelDocument checked No... display TextBox for input of Quantity of item
        {
            rbIsRequired.Visibility = Visibility.Hidden;
            txtAddQuantity.Visibility = Visibility.Visible;
            lblAddQuantityOrRequired.Content = "Quantity";
        }


        private void cbNewCountry_SelectionChanged(object sender, SelectionChangedEventArgs e) // To make sure that TravelDocument is required or not
        {
            string userLocation = signedInUser.Location.ToString();
            string destinationLocation = ((Countries)cbNewCountry.SelectedValue).ToString();
            // Travel Documents is not required
            if (userLocation == destinationLocation || (Enum.GetNames(typeof(EuropeanCountry)).Contains(userLocation)) & Enum.GetNames(typeof(EuropeanCountry)).Contains(destinationLocation))
            {
                // If we have same location as our destination
                // OR if we live within EU and will travel within EU
                rbRequiredFalse.IsChecked = true;
            }
            // Travel Documents is required
            else
            {
                // If the location as our destination is NOT the same,
                // OR if we live within EU and will travel outside EU,
                // OR if we live outside EU and will travel to another country
                rbRequiredTrue.IsChecked = true;
            }
        }

        private void AddPassportIfRequired() // The Passport will automaticly be added to the bag with status of required
        {
            string userLocation = signedInUser.Location.ToString();
            string destinationLocation = ((Countries)cbNewCountry.SelectedIndex - 1).ToString();
            // Travel Documents is not required
            if (userLocation == destinationLocation || (Enum.GetNames(typeof(EuropeanCountry)).Contains(userLocation)) & Enum.GetNames(typeof(EuropeanCountry)).Contains(destinationLocation))
            {
                TravelDocument travelDocument = new("Passport", false);

                ListViewItem item = new();
                item.Tag = travelDocument;
                item.Content = travelDocument.GetInfo();
                lstAddedPacklist.Items.Add(item);
            }
            // Travel Documents is required
            else
            {
                TravelDocument travelDocument = new("Passport", true);

                ListViewItem item = new();
                item.Tag = travelDocument;
                item.Content = travelDocument.GetInfo();
                lstAddedPacklist.Items.Add(item);
            }
        }

        private void btnAddPacklist_Click(object sender, RoutedEventArgs e) // Adds item to PackingList
        {
            // Gather inputs
            ListViewItem item = new();
            string packingItem = txtAddItem.Text;

            if (String.IsNullOrEmpty(packingItem))
            {
                MessageBox.Show("Please enter the name of the item!", "Packinglist Warning");
            }
            else if (rbTravelDocumentTrue.IsChecked == true) // If its TravelDocument...
            {
                if (packingItem.ToLower().Trim() == "passport" || packingItem.ToLower().Trim() == "pass") // ... Make sure passport its not added
                {
                    MessageBox.Show("You dont need to pack your Passport!\nWe will automaticly do that for you and set the Required State!", "Packinglist Warning");
                    return;
                }
                bool isRequired = false;

                if (rbRequiredTrue.IsChecked == true) // Makes sure the required status is true or false
                {
                    isRequired = true;
                }

                // Capitalizing first letter of the items word
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                string capitalizePackingItem = textInfo.ToTitleCase(packingItem);

                TravelDocument newTravelDocument = new(capitalizePackingItem, isRequired);

                item.Content = newTravelDocument.GetInfo();
                item.Tag = newTravelDocument;

                MessageBox.Show("New travel document added!", "Travel document added");
                lstAddedPacklist.Items.Add(item);

                ClearPackinglistInput();
            }
            else if (rbTravelDocumentFalse.IsChecked == true) // If its OtherItems...
            {
                if (String.IsNullOrEmpty(txtAddQuantity.Text))
                {
                    MessageBox.Show("Please enter quantity of items!", "Packinglist Warning");
                }
                else
                {
                    int packingQuantity = 0;

                    try
                    {
                        packingQuantity = Convert.ToInt32(txtAddQuantity.Text);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Please enter whole numbers in quantity!", "Packinglist Warning");
                        return;
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Your backpack is too small! Please enter a smaller quantity!", "Packinglist Warning");
                        return;
                    }

                    // Capitalizing the first letter of the item word
                    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                    string convertedPackingItem = textInfo.ToTitleCase(packingItem);

                    OtherItem otherItem = new(convertedPackingItem, packingQuantity);

                    item.Content = otherItem.GetInfo();
                    item.Tag = otherItem;

                    MessageBox.Show("New packing item added!", "Packing item added");
                    lstAddedPacklist.Items.Add(item);
                    ClearPackinglistInput();
                }
            }
        }

        private void btnRemovePacklist_Click(object sender, RoutedEventArgs e) // Removes item from PackingList
        {
            ListViewItem selectedItem = (ListViewItem)lstAddedPacklist.SelectedItem;

            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove {((IPackingListItem)selectedItem.Tag).Name} from your packinglist?", "Packinglist Warning", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Confirms for the user that the item is removed
                    MessageBox.Show($"{((IPackingListItem)selectedItem.Tag).Name} removed!", "Item removed");

                    lstAddedPacklist.Items.Remove(selectedItem);
                }
            }

        }
        private void btnAddTrip_Click(object sender, RoutedEventArgs e) // Adds trip to users Travels-list
        {
            // Gather all inputs

            if (cbNewCountry.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose a country to visit!", "Warning");
                return;
            }

            Countries newCountry = (Countries)cbNewCountry.SelectedItem;
            string newCity = txtNewCity.Text;
            DateTime startDate;
            DateTime endDate;
            int daysDifference;
            int newAmountTravelers;
            bool isAllInclusive;
            string newMeetingDetails;

            try
            {
                startDate = dpAddFromTravelDate.SelectedDate.Value;
                endDate = dpAddToTravelDate.SelectedDate.Value;
                daysDifference = (int)(endDate - startDate).TotalDays;

                if (daysDifference < 0)
                {
                    MessageBox.Show("You have entered the traveling dates incorrectly!", "Warning");
                    return;
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Please enter traveling dates!", "Warning");
                return;
            }


            if (String.IsNullOrEmpty(newCity))
            {
                MessageBox.Show("Please enter a city to visit!", "Warning");
                return;
            }

            try
            {
                newAmountTravelers = Convert.ToInt32(txtNewAmmountTravelers.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter whole numbers in TravelPals!", "Warning");
                return;
            }
            catch (OverflowException)
            {
                MessageBox.Show("We love all TravelPals, but it's too much for us to handle!\n" +
                    "Please enter a smaller number of TravelPals!", "TravelPals Warning");
                return;

            }

            if (cbNewTypeTrip.SelectedIndex == 1) // Type of trip is .... Vacation
            {
                if (rbAllInclusiveTrue.IsChecked == true)
                {
                    isAllInclusive = true;
                }
                else if (rbAllInclusiveFalse.IsChecked == true)
                {
                    isAllInclusive = false;
                }
                else
                {
                    MessageBox.Show("Please choose if it's all inclusive!", "Warning");
                    return;
                }

                AddPassportIfRequired();

                // Adds the trip to users Travel, and if there is any PackingListItem... this will be added in the list
                Vacation addingUserTrip = new(newCity, newCountry, newAmountTravelers, new List<IPackingListItem> { }, startDate, endDate, isAllInclusive);
                foreach (ListViewItem tripItem in lstAddedPacklist.Items)
                {
                    IPackingListItem vacationTrip = (IPackingListItem)tripItem.Tag;
                    addingUserTrip.PackingList.Add(vacationTrip);
                }

                // Adds the new trip to the users list

                ((User)UserManager.signedInUser).Travels.Add(addingUserTrip);
                MessageBox.Show("New trip added! Returning to menu!", "Trip added");
                TravelsWindow travelsWindow = new();
                travelsWindow.Show();
                Close();

            }
            else if (cbNewTypeTrip.SelectedIndex == 2) // Type of trip is .... Work Trip
            {
                newMeetingDetails = txtNewMeetingDetails.Text;

                if (String.IsNullOrEmpty(newMeetingDetails))
                {
                    MessageBox.Show("Please enter meeting details!", "Warning");
                    return;
                }

                AddPassportIfRequired();

                // Adds the trip to users Travel, and if there is any PackingListItem... this will be added in the list
                WorkTrip addingUserTrip = new WorkTrip(newCity, newCountry, newAmountTravelers, new List<IPackingListItem> { }, startDate, endDate, newMeetingDetails);
                foreach (ListViewItem tripItem in lstAddedPacklist.Items)
                {
                    IPackingListItem workTrp = (IPackingListItem)tripItem.Tag;
                    addingUserTrip.PackingList.Add(workTrp);
                }

                // Adds the new trip to the users list
                ((User)UserManager.signedInUser).Travels.Add(addingUserTrip);
                MessageBox.Show("New trip added! Returning to menu!", "Trip added");
                TravelsWindow travelsWindow = new();
                travelsWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Please choose the type of trip!", "Warning");
            }
        }
    }
}
