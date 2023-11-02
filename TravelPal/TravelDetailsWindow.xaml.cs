using System;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Classes;
using TravelPal.Manager;

namespace TravelPal
{
    public partial class TravelDetailsWindow : Window
    {
        IUser signedInUser = UserManager.signedInUser;

        public TravelDetailsWindow(ListBoxItem item)
        {
            InitializeComponent();
            UpdateUi(item);
        }

        private void UpdateUi(ListBoxItem item)
        {
            if (item != null)
            {
                Travel travelItem = (Travel)item.Tag;
                lblCountry.Content = travelItem.Country;
                txtCountry.Text = travelItem.Country.ToString();
                txtCity.Text = travelItem.Destination;
                txtAmmountTravelers.Text = travelItem.Travellers.ToString();
                dpFromTravelDate.Text = travelItem.StartDate.ToString();
                dpToTravelDate.Text = travelItem.EndDate.ToString();

                // Ändra dates
                DateTime startDate = dpFromTravelDate.SelectedDate.Value;
                DateTime endDate = dpToTravelDate.SelectedDate.Value;
                int daysDifference = (int)(endDate - startDate).TotalDays;

                txtLengthOfTrip.Text = daysDifference.ToString();


                if (travelItem.GetType().Name == "Vacation")
                {
                    txtPurposeOfTrip.Text = "Vacation";
                    lblPurposeOfTrip.Content = "All inclusive?";
                    rbIsAllInclusive.Visibility = Visibility.Visible;
                    txtMeetingDetails.Visibility = Visibility.Hidden;

                    Vacation vacationItem = (Vacation)travelItem;

                    if (vacationItem.AllInclusive == true)
                    {
                        rbAllInclusiveTrue.IsChecked = true;
                    }
                    else
                    {
                        rbAllInclusiveFalse.IsChecked = true;
                    }

                }
                else if (travelItem.GetType().Name == "WorkTrip")
                {
                    txtPurposeOfTrip.Text = "Work trip";
                    lblPurposeOfTrip.Content = "Meeting Details";
                    txtMeetingDetails.Visibility = Visibility.Visible;
                    rbIsAllInclusive.Visibility = Visibility.Hidden;

                    WorkTrip workTripItem = (WorkTrip)travelItem;
                    txtMeetingDetails.Text = workTripItem.MeetingDetails;
                }

                lstPacklist.Items.Clear();

                foreach (var packItem in travelItem.PackingList)
                {
                    ListViewItem packListItem = new();
                    if (packItem.GetType().ToString() == (typeof(TravelDocument).ToString()))
                    {
                        TravelDocument travelDocument = (TravelDocument)packItem;
                        packListItem.Content = $"Document: {travelDocument.Name} - Required: {(travelDocument.Required ? "Yes" : "No")}";
                        packListItem.Tag = packItem;

                        lstPacklist.Items.Add(packListItem);
                    }
                    else if (packItem.GetType().ToString() == (typeof(OtherItem).ToString()))
                    {
                        OtherItem otherItem = (OtherItem)packItem;
                        packListItem.Content = $"Item: {otherItem.Name} - Quantity: {otherItem.Quantity}";
                        packListItem.Tag = packItem;

                        lstPacklist.Items.Add(packListItem);
                    }
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travelsWindow = new TravelsWindow();
            travelsWindow.Show();
            Close();
        }

        private void lstPacklist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)lstPacklist.SelectedItem;

            if (selectedItem != null)
            {
                IPackingListItem selectedPackingItem = (IPackingListItem)selectedItem.Tag;

                txtItem.Text = selectedPackingItem.Name;

                if (selectedPackingItem.GetType().ToString() == (typeof(TravelDocument).ToString()))
                {
                    rbTravelDocumentTrue.IsChecked = true;

                    lblQuantityOrRequired.Content = "Required?";

                    TravelDocument travelDocument = (TravelDocument)selectedPackingItem;

                    rbIsRequired.Visibility = Visibility.Visible;
                    txtQuantity.Visibility = Visibility.Hidden;

                    if (travelDocument.Required == true)
                    {
                        rbRequiredTrue.IsChecked = true;
                    }
                    else
                    {
                        rbRequiredFalse.IsChecked = true;
                    }

                }
                else if (selectedPackingItem.GetType().ToString() == (typeof(OtherItem).ToString()))
                {
                    rbTravelDocumentFalse.IsChecked = true;
                    lblQuantityOrRequired.Content = "Quantity";

                    OtherItem otherItem = (OtherItem)selectedPackingItem;

                    txtQuantity.Visibility = Visibility.Visible;
                    rbIsRequired.Visibility = Visibility.Hidden;

                    txtQuantity.Text = otherItem.Quantity.ToString();
                }
            }
        }

        private void btnEditSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
