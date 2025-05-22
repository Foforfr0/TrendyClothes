using System.Windows;
using WpfApp.Pages.User.Auth;

namespace WpfApp.Pages {
    /// <summary>
    /// Interaction logic for WindowContainer.xaml
    /// </summary>
    public partial class WindowContainer : Window {
        public WindowContainer () {
            InitializeComponent ();

            MainFrame.Navigate (new Login ());
        }
    }
}
