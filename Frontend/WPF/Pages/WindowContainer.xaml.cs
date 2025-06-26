using System.Windows;
using System.Windows.Controls;
using WpfApp.Components;
using WpfApp.Pages.Home;
using WpfApp.Pages.User.Auth;
using WpfApp.Services.User.Auth;
using WpfApp.Utilities;

namespace WpfApp.Pages {
    public partial class WindowContainer : Window {
        public WindowContainer () {
            InitializeComponent ();
            NavigationManager.Initialize(MainFrame);

            NavigateToPage("Glb_Home", new HomePage());
            Loaded += WindowContainer_Loaded;
        }
        private void WindowContainer_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
        }
        public void NavigateToPage(string pageName, Page pageInstance)
        {
            NavigationManager.Instance.NavigateToPage(pageName, pageInstance);
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            //TODO: reload HomePage
            NavigateToPage("Glb_Home", new HomePage());
        }

        private void BtnCategories_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Load Categories UserControl
        }

        private void BtnOffers_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Load OffersPage
        }

        private void BtnTrends_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Load TrendPage
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //TODO: Load searchResults, perhaps another page
        }
        private void PopUpOverlay_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource == PopUpOverlay)
            {
                PopUpOverlay.Visibility = Visibility.Collapsed;
                PopUpHost.Content = null;
            }

            e.Handled = true;
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            UserSettings.Show(sender as FrameworkElement);
        }

        private void BtnShoppingCart_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Load ShoppingCartPage
        }
    }
}
