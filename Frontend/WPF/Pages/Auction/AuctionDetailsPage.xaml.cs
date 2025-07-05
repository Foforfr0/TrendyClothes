using System.Windows;
using System.Windows.Controls;
using WpfApp.Utilities;

namespace WpfApp.Pages.Auction {
    /// <summary>
    /// Lógica de interacción para AuctionDetailsPage.xaml
    /// </summary>
    public partial class AuctionDetailsPage : Page {
        public AuctionDetailsPage () {
            InitializeComponent ();
        }

        private void SendMessageToAuctioner_Click (object sender, RoutedEventArgs e) {
            //TODO open messages popup
        }

        private void IncreaseBid_Click (object sender, RoutedEventArgs e) {

        }

        private void BtnCancelBid_Click (object sender, RoutedEventArgs e) {
            NavigationManager.Instance.GoBack ();
        }
    }
}
