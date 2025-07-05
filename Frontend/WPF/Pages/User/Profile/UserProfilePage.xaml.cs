using GetImageProduct;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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

namespace WpfApp.Pages.User.Profile {
    public partial class UserProfilePage : Page {
        private ProductoAPIModel? _selectedProduct;
        private GetImageService.GetImageServiceClient _imageServiceClient;

        public UserProfilePage () {
            InitializeComponent ();
            LoadUserProfile ();
            LoadUserProducts ();
        }

        private async void LoadUserProducts () {
            try {
                string? username = UserSession.Instance.Username;
                string? token = UserSession.Instance.JwtToken;

                if (string.IsNullOrWhiteSpace (username) || string.IsNullOrWhiteSpace (token)) {
                    MessageDialog.Show ("Error", "No hay sesión activa.", AlertType.ERROR);
                    return;
                }

                string url = ProductEndpoints.GetMyProducts (username);
                var response = await HttpClientHelper.GetAsync (url, token);

                if (!response.IsSuccessStatusCode) {
                    MessageDialog.Show ("Error", "No se han podido cargar tus productos", AlertType.ERROR);
                    return;
                }

                var content = await response.Content.ReadFromJsonAsync<ProductListResponse> ();

                if (content?.Body == null || content.Body.Count == 0) {
                    MessageDialog.Show ("Aviso", "No tienes productos publicados.", AlertType.WARNING);
                    return;
                }

                ItemsFeed.Items.Clear ();
                _imageServiceClient = App.Services.GetRequiredService<GetImageService.GetImageServiceClient> ();

                foreach (var product in content.Body) {
                    var grpcResponse = await _imageServiceClient.GetImageAsync (new GetImageRequest {
                        ProductId = product.Id
                    });

                    byte[] imageBytes = grpcResponse.ImageData.ToByteArray ();

                    // Convertir byte[] a ImageSource (WPF)
                    BitmapImage bitmap = new BitmapImage ();
                    using (var ms = new MemoryStream (imageBytes)) {
                        bitmap.BeginInit ();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = ms;
                        bitmap.EndInit ();
                        bitmap.Freeze ();
                    }

                    var card = new ItemCard2 {
                        isSelectable = true,
                        DataContext = product,
                        Margin = new Thickness (15)
                    };

                    if (card.FindName ("ItemImage") is Image imageControl) {
                        imageControl.Source = bitmap;
                    }

                    card.CardSelected += Card_Selected;

                    ItemsFeed.Items.Add (card);
                }
            } catch (Exception ex) {
                MessageDialog.Show ("GlbDialogT_NoConnection", $"GlbDialogD_NoConnection: {ex.Message}",
                    AlertType.ERROR);
            }
        }

        private async void LoadUserProfile () {
            try {
                string? username = UserSession.Instance.Username;
                string? token = UserSession.Instance.JwtToken;

                if (string.IsNullOrWhiteSpace (username) || string.IsNullOrWhiteSpace (token)) {
                    MessageDialog.Show ("GlbDialogT_SessionError", "GlbDialogD_SessionError",
                        AlertType.ERROR);
                    return;
                }

                string url = ProfileEndpoints.GetPersonalData (username!);
                var response = await HttpClientHelper.GetAsync (url, token);

                if (!response.IsSuccessStatusCode) {
                    MessageDialog.Show ("GlbDialogT_SessionError", "GlbDialogD_SessionError",
                        AlertType.ERROR);
                    return;
                }

                var content = await response.Content.ReadFromJsonAsync<ApiResponse<PersonalInformationDTO>> ();

                if (content?.body == null) {
                    MessageDialog.Show ("GlbDialogT_SessionError", "GlbDialogD_SessionError",
                        AlertType.ERROR);
                    return;
                }

                TbFullName.Text = content.body.FullName ?? "N/A";
                TbUsername.Text = content.body.Username ?? "N/A";
                TbEmail.Text = content.body.Email ?? "N/A";
                string areaCode = content.body.AreaCode ?? "";
                string phone = content.body.PhoneNumber ?? "";

                TbPhone.Text = $"+{areaCode} {phone}".Trim ();
                TbUserRole.Text = content.body.Role ?? "N/A";
            } catch (Exception ex) {
                MessageDialog.Show ("GlbDialogT_NoConnection", $"GlbDialogD_NoConnection {ex.Message}", AlertType.ERROR);
            }
        }

        private void Card_Selected (object sender, EventArgs e) {
            if (sender is ItemCard2 selectedCard && selectedCard.DataContext is ProductoAPIModel product) {
                _selectedProduct = product;

                BtnDeletePost.Visibility = Visibility.Visible;
                BtnEditPost.Visibility = Visibility.Visible;
            }
        }


        private void BtnEditProfile_Click (object sender, RoutedEventArgs e) {
            NavigationManager.Instance.NavigateToPage ("Glb_EditProfile", new EditUserProfile ());
        }

        private void BtnRegisterProduct_Click (object sender, RoutedEventArgs e) {
            NavigationManager.Instance.NavigateToPage ("Glb_RegisterItem", new RegisterProductPage ());
        }

        private void BtnStartAuction_Click (object sender, RoutedEventArgs e) {
            NavigationManager.Instance.NavigateToPage ("Glb_RegisterAuction", new RegisterAuctionPage ());
        }

        private void BtnMessages_Click (object sender, RoutedEventArgs e) {

        }

        private void BtnEditPost_Click (object sender, RoutedEventArgs e) {
            //TODO Load post details, then redirect to EditProductPage(itemToEdit) 
            NavigationManager.Instance.NavigateToPage ("EditItem_Header", new RegisterProductPage ());
        }

        private async void BtnDeletePost_Click (object sender, RoutedEventArgs e) {
            if (_selectedProduct == null) {
                MessageDialog.Show ("Error", "No se ha seleccionado ningún producto.", AlertType.ERROR);
                return;
            }

            MessageDialog.ShowConfirm (
                "EditItem_DialogTDelete",
                "EditItem_DialogDDelete",
                onConfirm: async () => {
                    try {
                        string url = ProductEndpoints.DeleteProduct (_selectedProduct.Id);
                        string? token = UserSession.Instance.JwtToken;

                        if (string.IsNullOrWhiteSpace (token)) {
                            MessageDialog.Show ("Error", "No hay sesión activa.", AlertType.ERROR);
                            return;
                        }

                        var response = await HttpClientHelper.DeleteAsync (url, token);

                        if (!response.IsSuccessStatusCode) {
                            MessageDialog.Show ("Error", "No se pudo eliminar el producto.", AlertType.ERROR);
                            return;
                        }

                        // Success: Reload or navigate
                        NavigationManager.Instance.NavigateToPage ("Items_Header", new UserProfilePage ());

                    } catch (Exception ex) {
                        MessageDialog.Show ("Error", $"Error inesperado: {ex.Message}", AlertType.ERROR);
                    }
                });
        }
    }
}
