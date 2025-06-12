using System.Windows;
using WpfApp.Pages.User.Auth;
using WpfApp.Services.User.Auth;

namespace WpfApp.Pages {
    public partial class WindowContainer : Window {
        public WindowContainer () {
            InitializeComponent ();
            //MainFrame.Navigate (new Login (_loginService));
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            /*TODO:
             * Reload Index on click
             * Create NavigationManager
             */
        }
    }
}
