using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using WebPage.Connections;
using WpfApp.Components;
using WpfApp.DTO;
using WpfApp.DTO.Products;
using WpfApp.DTO.User.Profile;
using WpfApp.Pages.Auction;
using WpfApp.Pages.Dialogs;
using WpfApp.Pages.Product;
using WpfApp.Session;
using WpfApp.Utilities;

namespace WpfApp.Pages.User.Profile
{
    public partial class UserProfilePage : Page
    {
        public UserProfilePage()
        {
            InitializeComponent();
            LoadUserProfile();
            LoadUserProducts();
        }

        private async void LoadUserProducts()
        {
            try
            {
                string? username = UserSession.Instance.Username;
                string? token = UserSession.Instance.JwtToken;

                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(token))
                {
                    MessageDialog.Show("Error", "No hay sesion activa", AlertType.ERROR);
                    return;
                }

                string url = ProductEndpoints.GetMyProducts(username);
                var response = await HttpClientHelper.GetAsync(url, token);

                if (!response.IsSuccessStatusCode)
                {
                    MessageDialog.Show("Error", "No se han podido cargar tus productos",
                        AlertType.ERROR);
                    return;
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var productResponse = JsonSerializer.Deserialize<ProductListResponse>
                    (jsonString, options);

                if (productResponse?.Body == null || productResponse.Body.Count == 0)
                {
                    ItemsFeed.ItemsSource = new List<ProductoAPIModel>();
                    return;
                }

                ItemsFeed.ItemsSource = productResponse.Body;
            }
            catch (Exception ex)
            {
                MessageDialog.Show("GlbDialogT_NoConnection", $"GlbDialogD_NoConnection: {ex.Message}",
                    AlertType.ERROR);
            }
        }

        private async void LoadUserProfile()
        {
            try
            {
                string? username = UserSession.Instance.Username;
                string? token = UserSession.Instance.JwtToken;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
                {
                    MessageDialog.Show("GlbDialogT_SessionError", "GlbDialogD_SessionError",
                        AlertType.ERROR);
                    return;
                }

                string url = ProfileEndpoints.GetPersonalData(username!);
                var response = await HttpClientHelper.GetAsync(url, token);

                if (!response.IsSuccessStatusCode)
                {
                    MessageDialog.Show("GlbDialogT_SessionError", "GlbDialogD_SessionError",
                        AlertType.ERROR);
                    return;
                }

                var content = await response.Content.ReadFromJsonAsync<ApiResponse<PersonalInformationDTO>>();

                if (content?.body == null)
                {
                    MessageDialog.Show("GlbDialogT_SessionError", "GlbDialogD_SessionError",
                        AlertType.ERROR);
                    return;
                }

                TbFullName.Text = content.body.FullName ?? "N/A";
                TbUsername.Text = content.body.Username ?? "N/A";
                TbEmail.Text = content.body.Email ?? "N/A";
                string areaCode = content.body.AreaCode ?? "";
                string phone = content.body.PhoneNumber ?? "";

                TbPhone.Text = $"+{areaCode} {phone}".Trim();
                TbUserRole.Text = content.body.Role ?? "N/A";
            }
            catch (Exception ex)
            {
                MessageDialog.Show("GlbDialogT_NoConnection", $"GlbDialogD_NoConnection {ex.Message}", AlertType.ERROR);
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
