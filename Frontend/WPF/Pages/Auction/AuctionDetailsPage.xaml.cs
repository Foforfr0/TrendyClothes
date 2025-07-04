using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.Pages.Dialogs;
using WpfApp.Utilities;

namespace WpfApp.Pages.Auction
{
    /// <summary>
    /// Lógica de interacción para AuctionDetailsPage.xaml
    /// </summary>
    public partial class AuctionDetailsPage : Page
    {
        public AuctionDetailsPage()
        {
            InitializeComponent();
        }

        private void SendMessageToAuctioner_Click(object sender, RoutedEventArgs e)
        {
            //TODO open messages popup
        }
        
        private void IncreaseBid_Click(object sender, RoutedEventArgs e)
        {

        }       
        
        private void BtnCancelBid_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.GoBack();
        }
    }
}
