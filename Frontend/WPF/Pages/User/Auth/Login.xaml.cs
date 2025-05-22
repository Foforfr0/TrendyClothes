using System.Windows.Controls;

namespace WpfApp.Pages.User.Auth {
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page {
        public Login () {
            InitializeComponent ();
        }

        private void CheckInformation (object sender, System.Windows.RoutedEventArgs e) {
            NavigationService.Navigate (new Layout ());
        }
    }
}
