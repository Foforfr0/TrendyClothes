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
using WpfApp.Utilities;

namespace WpfApp.Pages.Auction
{
    /// <summary>
    /// Lógica de interacción para RegisterAuctionPage.xaml
    /// </summary>
    public partial class RegisterAuctionPage : Page
    {
        public RegisterAuctionPage()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.GoBack();
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RequiredFields_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnStartAcution_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CbAuctionDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
