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
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            Close();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            //string username;

            //try
            //{
            //    username = txtUsername.Text;
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Please fill in the fields!", "Warning");
            //}
            //string password;

            string username = txtUsername.Text;
            string password = txtPassword.Password;

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
