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
using WpfApp.Pages.Auction;
using WpfApp.Pages.Home;
using WpfApp.Utilities;

namespace WpfApp.Components
{
    /// <summary>
    /// Lógica de interacción para AuctionCard.xaml
    /// </summary>
    public partial class AuctionCard : UserControl
    {
        public AuctionCard()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationManager.Instance.NavigateToPage("AuctionDetails_Header", new AuctionDetailsPage());
        }
    }
}
