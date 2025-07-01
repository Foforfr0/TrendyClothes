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
using WpfApp.Components;
using WpfApp.Utilities;

namespace WpfApp.Pages.Home
{
    /// <summary>
    /// Lógica de interacción para AuctionsPage.xaml
    /// </summary>
    public partial class AuctionsPage : Page
    {
        public AuctionsPage()
        {
            InitializeComponent();
            LoadMockAuctionCards();
        }

        private void LoadMockAuctionCards()
        {
            for (int i = 0; i < 10; i++)
            {
                var card = new AuctionCard();
                AuctionFeed.Items.Add(card);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.GoBack();
        }
    }
}
