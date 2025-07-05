using System.Windows;
using System.Windows.Controls;
using WpfApp.Pages.Dialogs;
using WpfApp.Utilities;

namespace WpfApp.Pages.Auction {
    /// <summary>
    /// Lógica de interacción para RegisterAuctionPage.xaml
    /// </summary>
    public partial class RegisterAuctionPage : Page {
        public RegisterAuctionPage () {
            InitializeComponent ();
        }

        private void BtnBack_Click (object sender, RoutedEventArgs e) {
            NavigationManager.Instance.GoBack ();
        }

        private void BtnSelectImage_Click (object sender, RoutedEventArgs e) {

        }

        private void BtnDeleteImage_Click (object sender, RoutedEventArgs e) {

        }

        private void RequiredFields_TextChanged (object sender, TextChangedEventArgs e) {

        }

        private void BtnCancel_Click (object sender, RoutedEventArgs e) {
            MessageDialog.ShowConfirm (
                "Glb_DialogTDiscard",
                "Glb_DialogDDiscard",
                onConfirm: () => { NavigationManager.Instance.GoBack (); });
        }

        private void BtnStartAcution_Click (object sender, RoutedEventArgs e) {

        }

        private void CbAuctionDuration_SelectionChanged (object sender, SelectionChangedEventArgs e) {

        }
    }
}
