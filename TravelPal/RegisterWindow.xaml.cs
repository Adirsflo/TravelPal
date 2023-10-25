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
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New user registered! Welcome!", "New account");
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }
    }
}
