using System.Windows;

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

            TravelsWindow travelsWindow = new TravelsWindow();
            travelsWindow.Show();
            Close();
        }
    }
}
