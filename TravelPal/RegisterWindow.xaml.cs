using System.Windows;

namespace TravelPal
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Makes sure user ACTUALLY wants to cancel registration
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

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Gather variables
            string username = txtNewUsername.Text.ToLower();
            string firstName = txtNewFirstName.Text;
            string lastName = txtNewLastName.Text;
            string country = cbNewCountry.Text;
            string newPassword = txtNewPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            // Make sure that the Username is not taken
            // If username is taken... "Username is already taken"


            // Make sure that no numbers is typed in FirstName and LastName
            // If Names has numbers... "Names can't involve numbers"

            // Format the FirstName and LastName to first letter to capital letter

            // Make sure that the password is matching
            // If password is not matching... "The password doesn't match!"





            // Add user to userList
            /*Customer newCustomer = new(username, newPassword, firstName, lastName, age);

            newCustomer.FirstName = firstName;
            newCustomer.LastName = lastName;
            newCustomer.Age = age;

            UserManager.userList.Add(new Customer(username, newPassword, firstName, lastName, age));
            */


            // Message when new account is registered
            MessageBox.Show("New user registered! Welcome!", "New account");
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }
    }
}
