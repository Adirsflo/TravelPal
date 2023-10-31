using System;
using System.Windows;
using System.Windows.Controls;
using TravelPal.Enums;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for AddTravelWindow.xaml
    /// </summary>
    public partial class AddTravelWindow : Window
    {
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
            dpAddToTravelDate.DisplayDateStart = DateTime.Now;

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
    }
}
