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
using WpfApp.Pages.Auction;
using WpfApp.Pages.Dialogs;
using WpfApp.Pages.Product;
using WpfApp.Utilities;

namespace WpfApp.Pages.User.Profile
{
    public partial class UserProfilePage : Page
    {
        public UserProfilePage()
        {
            InitializeComponent();
            LoadMockItemCards();
        }

        //TODO: Load user products 
        private void LoadMockItemCards()
        {
            for (int i = 0; i < 10; i++)
            {
                var card = new ItemCard2()
                {
                    isSelectable = true
                };
                card.CardSelected += Card_Selected;
                ItemsFeed.Items.Add(card);
            }
        }

        private void Card_Selected(object sender, EventArgs e)
        {
            var selectedCard = sender as ItemCard2;
            if (selectedCard != null)
            {
                BtnDeletePost.Visibility = Visibility.Visible;
                BtnEditPost.Visibility = Visibility.Visible;
            }
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
            //TODO Load post details, then redirect to EditProductPage(itemToEdit) 
            NavigationManager.Instance.NavigateToPage("EditItem_Header", new RegisterProductPage());
        }

        private void BtnDeletePost_Click(object sender, RoutedEventArgs e)
        {
            //TODO: call method to delete and reload page
            MessageDialog.ShowConfirm(
                "EditItem_DialogTDelete", "EditItem_DialogDDelete",
                onConfirm: () => { NavigationManager.Instance.NavigateToPage("Items_Header", new UserProfilePage()); });
        }
    }
}
