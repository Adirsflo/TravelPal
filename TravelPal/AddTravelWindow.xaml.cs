using System;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Classes;
using TravelPal.Enums;
using TravelPal.Manager;

namespace TravelPal
{
    public partial class AddTravelWindow : Window
    {
        IUser signedInUser = UserManager.signedInUser;
        public AddTravelWindow()
        {
            InitializeComponent();

            UpdateUi();
        }

        private void UpdateUi()
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
        }
        private void CalculateAndDisplayDifference()
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

        private void dpAddToTravelDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateAndDisplayDifference();
        }
        private void dpAddFromTravelDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateAndDisplayDifference();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            TravelsWindow travelsWindow = new TravelsWindow();
            travelsWindow.Show();
            Close();
        }

        private void cbNewTypeTrip_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void rbTravelDocumentTrue_Checked(object sender, RoutedEventArgs e)
        {
            rbIsRequired.Visibility = Visibility.Visible;
            txtAddQuantity.Visibility = Visibility.Hidden;
            lblAddQuantityOrRequired.Content = "Required?";
        }

        private void rbTravelDocumentFalse_Checked(object sender, RoutedEventArgs e)
        {
            rbIsRequired.Visibility = Visibility.Hidden;
            txtAddQuantity.Visibility = Visibility.Visible;
            lblAddQuantityOrRequired.Content = "Quantity";
        }

        private void cbNewCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Kolla om User är från Europe
            // Om den är det...
            // Kolla om den ska till ett land inom Europe

            string userLocation = signedInUser.Location.ToString();

            //string destinationLocation = ((Countries)cbNewCountry.SelectedItem - 1).ToString();

            //if (userLocation == destinationLocation || )
            //{

            //}

            // Travel Documents is not required
            if (signedInUser.Location == (Countries)cbNewCountry.SelectedIndex - 1 || (Enum.IsDefined(typeof(EuropeanCountry), cbNewCountry.SelectedItem) && Enum.IsDefined(typeof(EuropeanCountry), signedInUser.Location)))
            {
                // NO REQUIRED
                // Ifall det landet som personen reser till är från samma land
                // Ifall landet befinner sig i Europa, och reser från europa
                MessageBox.Show("This Works");
                rbRequiredFalse.IsChecked = true;
                break;
                // REQUIRED
                // Ifall landet inte reser inom Europa
                // Ifall landet inte är samma som personen kommer från
            }
            // Travel Documents is required
            if ((EuropeanCountry)signedInUser.Location != (EuropeanCountry)cbNewCountry.SelectedIndex || signedInUser.Location != (Countries)cbNewCountry.SelectedIndex)
            {
                rbRequiredTrue.IsChecked = true;
            }

        }
    }
}
