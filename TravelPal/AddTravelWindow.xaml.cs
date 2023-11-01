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
            cbNewCountry.Items.Add("--Country--");
            foreach (var country in Enum.GetValues(typeof(Countries)))
            {
                cbNewCountry.Items.Add(country);
            }
            cbNewCountry.SelectedIndex = 0;

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
            TravelsWindow travelsWindow = new TravelsWindow();
            travelsWindow.Show();
            Close();
        }

        private void cbNewTypeTrip_SelectionChanged(object sender, SelectionChangedEventArgs e) // Views the right TextBoxes for the right trip type
        {
            if (cbNewTypeTrip.SelectedIndex == 1)
            {
                txtNewMeetingDetails.Text = "";
                lblNewPurposeOfTrip.Visibility = Visibility.Visible;
                lblNewPurposeOfTrip.Content = "All Inclusive?";
                rbIsAllInclusive.Visibility = Visibility.Visible;
                txtNewMeetingDetails.Visibility = Visibility.Hidden;
            }
            else if (cbNewTypeTrip.SelectedIndex == 2)
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

        private void rbTravelDocumentTrue_Checked(object sender, RoutedEventArgs e) // If checked Yes... displays if the traveldocument is required
        {
            rbIsRequired.Visibility = Visibility.Visible;
            txtAddQuantity.Visibility = Visibility.Hidden;
            lblAddQuantityOrRequired.Content = "Required?";
        }

        private void rbTravelDocumentFalse_Checked(object sender, RoutedEventArgs e) // If checked No... display TextBox for input of quantity of item
        {
            rbIsRequired.Visibility = Visibility.Hidden;
            txtAddQuantity.Visibility = Visibility.Visible;
            lblAddQuantityOrRequired.Content = "Quantity";
        }

        private void cbNewCountry_SelectionChanged(object sender, SelectionChangedEventArgs e) // To make sure that TravelDocument is required or not
        {
            string userLocation = signedInUser.Location.ToString();
            string destinationLocation = ((Countries)cbNewCountry.SelectedIndex - 1).ToString();

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
                if (rbRequiredTrue.IsChecked == true)
                {
                    bool isRequired = true;

                    IPackingListItem packingListItem = (IPackingListItem)signedInUser;
                    TravelDocument documentItem = (TravelDocument)packingListItem;
                    documentItem.Name = packingItem;
                    documentItem.Required = isRequired;

                    item.Content = $"Document: {packingItem} - Required: Yes";
                    item.Tag = documentItem;

                    MessageBox.Show("New travel document added!", "Travel document added");
                    lstAddedPacklist.Items.Add(item);

                    ClearPackinglistInput();
                }
                else
                {
                    bool isRequired = false;

                    IPackingListItem packingListItem = (IPackingListItem)signedInUser;
                    TravelDocument documentItem = (TravelDocument)packingListItem;
                    documentItem.Name = packingItem;
                    documentItem.Required = isRequired;

                    item.Content = $"Document: {packingItem} - Required: No";
                    item.Tag = documentItem;

                    MessageBox.Show("New travel document added!", "Travel document added");
                    lstAddedPacklist.Items.Add(item);
                    ClearPackinglistInput();
                }
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
                        packingQuantity = Convert.ToInt16(txtAddQuantity.Text);
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Please enter whole numbers in quantity!", "Packinglist Warning");
                    }

                    if (int.TryParse(txtAddQuantity.Text, out int value))
                    {
                        // Lägg till i PackingListItem

                        IPackingListItem packingListItem = (IPackingListItem)signedInUser;
                        OtherItem otherItem = (OtherItem)packingListItem;
                        otherItem.Name = packingItem;
                        otherItem.Quantity = packingQuantity;


                        item.Content = $"Item: {packingItem} - Quantity: {packingQuantity}";
                        item.Tag = otherItem;

                        MessageBox.Show("New packing item added!", "Packing item added");
                        lstAddedPacklist.Items.Add(item);
                        ClearPackinglistInput();
                    }
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
                    // Ta bort Item från listan


                    // Bekräfta för användaren att den numera är bortagen
                    MessageBox.Show($"{selectedItem} removed!", "Item removed");
                    lstAddedPacklist.Items.Remove(selectedItem);


                }
            }

        }
    }
}
