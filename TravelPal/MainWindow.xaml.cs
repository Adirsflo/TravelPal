using System.Windows;
using TravelPal.Manager;

namespace TravelPal
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void blkRegister_Click(object sender, RoutedEventArgs e)
        {
            // Allows the user to register a new account
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            Close();
        }
        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            // Gather inputs
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Checks if user input was correct
            bool isSignedIn = UserManager.SignInUser(username, password);

            if (isSignedIn == true)
            {
                TravelsWindow travelsWindow = new TravelsWindow();
                travelsWindow.Show();
                Close();
            }
        }
    }
}
