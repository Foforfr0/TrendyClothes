using System.Windows;
using System.Windows.Controls;
using WpfApp.Pages.User.Auth;

namespace WpfApp.Pages {
    /// <summary>
    /// Interaction logic for WindowContainer.xaml
    /// </summary>
    public partial class WindowContainer : Window {
        private static Frame _mainFrame = new Frame ();

        public WindowContainer () {
            InitializeComponent ();

            _mainFrame = MainFrame;
            _mainFrame.Navigate (new Login ());
            //MainFrame.Navigate (new Login (_loginService));
        }

        public static void SetFrame (Page page) {
            _mainFrame.Navigate (page);
        }
    }
}
