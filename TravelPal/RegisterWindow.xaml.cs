using System;
using System.Globalization;
using System.Windows;
using TravelPal.Enums;
using TravelPal.Manager;

namespace TravelPal
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            cbNewCountry.Items.Add("--Select Country--");
            foreach (var country in Enum.GetValues(typeof(Countries)))
            {
                cbNewCountry.Items.Add(country);
            }
            cbNewCountry.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Makes sure user ACTUALLY wants to cancel registration
            CancelToMainWindow();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Gather variables
            string username = txtNewUsername.Text.ToLower();
            Countries country = (Countries)cbNewCountry.SelectedIndex;
            string newPassword = txtNewPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            // Capitalizing the first letter of each name
            string firstName = CapitalizeFirstLetter(txtNewFirstName.Text.ToLower());
            string lastName = CapitalizeFirstLetter(txtNewLastName.Text.ToLower());

            if (txtNewUsername.Text == "")
            {
                // If the username field is empty
                MessageBox.Show("Please enter a new username!", "Warning");
            }
            else if (txtNewFirstName.Text == "" || txtNewLastName.Text == "")
            {
                // If the name-fields are empty
                MessageBox.Show("Please fill in your name!", "Warning");
            }
            // Make sure that the user select country
            else if (cbNewCountry.SelectedIndex <= 0)
            {
                // If nothing is selected, send a warning
                MessageBox.Show("Please choose a country!", "Warning");
            }
            // If the password-fields are empty
            else if (newPassword == "" && confirmPassword == "")
            {
                MessageBox.Show("Please enter a new password!", "Warning");
            }
            // Make sure that the password is matching
            else if (newPassword != confirmPassword)
            {
                // If password is not matching... "The password doesn't match!"
                MessageBox.Show("The password doesn't match!", "Warning");
            }
            else
            {
                // Make sure that the Username is not taken
                bool isAddedUser = UserManager.AddUser(username, firstName, lastName, country, newPassword);

                if (isAddedUser)
                {
                    // Message when new account is registered
                    MessageBox.Show("New user registered! Welcome!", "New account");
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    // If username is taken, throw error
                }
            }
            // Make sure that no numbers is typed in FirstName and LastName
            // If Names has numbers... "Names can't involve numbers"
        }

        private void CancelToMainWindow()
        {
            if (txtNewUsername.Text != "" || txtNewFirstName.Text != "" || txtNewLastName.Text != "" || txtNewPassword.Password != "" || txtConfirmPassword.Password != "")
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel?\n\nYour progress will not be saved!", "Warning", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                    Close();
                }
            }
            else
            {
                MainWindow mainWindow = new();
                mainWindow.Show();
                Close();
            }
        }

        private string CapitalizeFirstLetter(string name)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(name);
        }
    }
}
