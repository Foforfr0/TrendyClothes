using System.Windows;
using WpfApp.Pages.User.Auth;
using WpfApp.Services.User.Auth;
using WpfApp.Utilities;

namespace WpfApp.Pages {
    public partial class WindowContainer : Window {
        public WindowContainer () {
            InitializeComponent ();
            NavigationManager.Initialize(MainFrame);
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            /*TODO:
             * Reload Index on click
             * Create NavigationManager
             */
        }

        private void BtnCategories_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOffers_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnShoppingCart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
