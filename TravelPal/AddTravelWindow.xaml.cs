using System;
using System.Collections.Generic;
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
            //foreach (var country in Enum.GetValues(typeof(Countries)))
            //{
            //    cbNewCountry.Items.Add(country);
            //}

            cbNewCountry.ItemsSource = Enum.GetValues(typeof(Countries));

            //cbNewCountry.Items.Insert(0, "--Country--");
            cbNewCountry.SelectedIndex = -1;

            // User can only pick from todays date
            dpAddFromTravelDate.DisplayDateStart = DateTime.Now;
            dpAddToTravelDate.DisplayDateStart = DateTime.Now; // TODO: Kan inte vara innan "From" datumet

            CalculateAndDisplayDifference();

            cbNewTypeTrip.Items.Add("--Type of Trip--");
            cbNewTypeTrip.Items.Add("Vacation");
            cbNewTypeTrip.Items.Add("Work Trip");
            cbNewTypeTrip.SelectedIndex = 0;

            ClearPackinglistInput();
        }
        private void blkInformation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Welcome to Add Travel-Window!\n\n" +
                "-Fill in the information about your trip below \n" +
                "-If you wish to add an item to your packlist, you can do that by clicking on \"Add\" button\n" +
                "-If you wish to remove an item from your packlist, select the item and click on \"Remove\" button\n" +
                "-When you are done filling in all the information about the trip, click on \"Add Travel\" button\n" +
                "-On your upper right corner, you can choose to view User-profile, or click on \"Back\" to return", "Information - Navigation");
        }
        private void blkUser_Click(object sender, RoutedEventArgs e)
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
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
            if (cbNewTypeTrip.SelectedIndex == 1) // Vacation
            {
                txtNewMeetingDetails.Text = "";
                lblNewPurposeOfTrip.Visibility = Visibility.Visible;
                lblNewPurposeOfTrip.Content = "All Inclusive?";
                rbIsAllInclusive.Visibility = Visibility.Visible;
                txtNewMeetingDetails.Visibility = Visibility.Hidden;
            }
            else if (cbNewTypeTrip.SelectedIndex == 2) // Work Trip
            {
                lblNewPurposeOfTrip.Content = "Meeting Details";
                lblNewPurposeOfTrip.Visibility = Visibility.Visible;
                txtNewMeetingDetails.Visibility = Visibility.Visible;
                rbIsAllInclusive.Visibility = Visibility.Hidden;
            }
            else
            {
                txtNewMeetingDetails.Text = "";
                lblNewPurposeOfTrip.Visibility = Visibility.Hidden;
                rbIsAllInclusive.Visibility = Visibility.Hidden;
                txtNewMeetingDetails.Visibility = Visibility.Hidden;
            }
        }

        private void rbTravelDocumentTrue_Checked(object sender, RoutedEventArgs e) // If TravelDocument checked Yes... displays if the traveldocument is required
        {
            rbIsRequired.Visibility = Visibility.Visible;
            txtAddQuantity.Visibility = Visibility.Hidden;
            lblAddQuantityOrRequired.Content = "Required?";
        }

        private void rbTravelDocumentFalse_Checked(object sender, RoutedEventArgs e) // If TravelDocument checked No... display TextBox for input of quantity of item
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
                // If we have same location as our destination OR if we live within EU and will travel within EU
                rbRequiredFalse.IsChecked = true;
            }
            // Travel Documents is required
            else
            {
                rbRequiredTrue.IsChecked = true;
            }
        }

        private void AddPassportIfRequired() // TODO: Denna ska läggas in i slutet innan allt läggs in i väskan
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
            else if (rbTravelDocumentTrue.IsChecked == true)
            {
                bool isRequired = false;

                if (rbRequiredTrue.IsChecked == true)
                {
                    isRequired = true;
                }

                TravelDocument newTravelDocument = new(packingItem, isRequired);


                item.Content = $"Document: {newTravelDocument.Name} - Required: {(newTravelDocument.Required ? "Yes" : "No")}";
                item.Tag = newTravelDocument;

                MessageBox.Show("New travel document added!", "Travel document added");
                lstAddedPacklist.Items.Add(item);

                ClearPackinglistInput();
            }
            else if (rbTravelDocumentFalse.IsChecked == true)
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

                    OtherItem otherItem = new(packingItem, packingQuantity);

                    item.Content = $"Item: {packingItem} - Quantity: {packingQuantity}";
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
                    // Bekräfta för användaren att den numera är bortagen
                    MessageBox.Show($"{((IPackingListItem)selectedItem.Tag).Name} removed!", "Item removed");

                    lstAddedPacklist.Items.Remove(selectedItem);
                }
            }

        }
        private void btnAddTrip_Click(object sender, RoutedEventArgs e) // Adds trip to users Travels-list
        {
            // Gather all inputs

            if (cbNewCountry.SelectedIndex <= 0)
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
                MessageBox.Show("We love all TravelPals, but it's too much for us to handle! Please enter a smaller number of group!", "TravelPals Warning");
                return;

            }

            if (cbNewTypeTrip.SelectedIndex == 1) // Vacation
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

                // TODO: Ändra till lämpliga namn
                Vacation userTrip = new(newCity, newCountry, newAmountTravelers, new List<IPackingListItem> { }, startDate, endDate, isAllInclusive);
                foreach (ListViewItem tripItem in lstAddedPacklist.Items)
                {
                    IPackingListItem vacationTrip = (IPackingListItem)tripItem.Tag;
                    userTrip.PackingList.Add(vacationTrip);
                }

                // Lägg till den nya resan till vår användares lista


                if (UserManager.signedInUser != null)
                {
                    ((User)UserManager.signedInUser).Travels.Add(userTrip);
                    MessageBox.Show("New trip added! Returning to menu!", "Trip added");
                    TravelsWindow travelsWindow = new();
                    travelsWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("ERROR", "Warning");
                }
            }
            else if (cbNewTypeTrip.SelectedIndex == 2) // Work Trip
            {
                newMeetingDetails = txtNewMeetingDetails.Text;

                if (String.IsNullOrEmpty(newMeetingDetails))
                {
                    MessageBox.Show("Please enter meeting details!", "Warning");
                    return;
                }

                AddPassportIfRequired();

                WorkTrip userTrip = new WorkTrip(newCity, newCountry, newAmountTravelers, new List<IPackingListItem> { }, startDate, endDate, newMeetingDetails);
                foreach (ListViewItem tripItem in lstAddedPacklist.Items)
                {
                    IPackingListItem workTrp = (IPackingListItem)tripItem.Tag;
                    userTrip.PackingList.Add(workTrp);
                }

                // Lägg till den nya resan till vår användares lista
                ((User)UserManager.signedInUser).Travels.Add(userTrip);
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
