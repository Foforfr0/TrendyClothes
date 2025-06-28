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
using WpfApp.Pages.Product;
using WpfApp.Utilities;

namespace WpfApp.Pages.User.Profile
{
    /// <summary>
    /// Lógica de interacción para UserProfilePage.xaml
    /// </summary>
    public partial class UserProfilePage : Page
    {
        public UserProfilePage()
        {
            InitializeComponent();
        }

        private void BtnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.NavigateToPage("Glb_EditProfile", new EditUserProfile());
        }

        private void BtnRegisterProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.NavigateToPage("Glb_RegisterItem", new RegisterProductPage());
        }

        private void BtnStartAuction_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.NavigateToPage("Glb_RegisterAuction", new RegisterAuctionPage());
        }

        private void BtnMessages_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEditPost_Click(object sender, RoutedEventArgs e)
        {
            //TODO Load post details, then redirect to EditProductPage()
        }

        private void BtnDeletePost_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
