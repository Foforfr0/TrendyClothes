using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Auctions;
using ClienteMAUI.Models.DTO.Pruducts;
using ClienteMAUI.Models.ViewModel;
using ClienteMAUI.Session;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClienteMAUI.Views;

public partial class UserProfilePage : ContentPage
{
    private readonly HttpClient _httpClient = new();
    private List<ProductoViewModel> _allProducts = new();
    private List<MyAuctionsDTO> _allAuctions = new();
    private bool _productsLoaded = false;
    private bool _AuctionsLoaded = false;

    private readonly List<string> _auctionStatuses = new()
    {
        "Todos", "Activo", "Pausado", "Cancelado", "Finalizado"
    };

	public UserProfilePage()
	{
		InitializeComponent();
	}

    private void InitializeAuctionStatusPicker()
    {
        AuctionStatusPicker.ItemsSource = _auctionStatuses;
        AuctionStatusPicker.SelectedIndex = 0; // Default to "Todos"
    }

    private async Task<ImageSource?> CargarImagenProductoAsync(int productId)
    {
        try
        {
            var imageUrl = ProductEndpoints.GetProductImage(productId);
            var imageResponse = await _httpClient.GetAsync(imageUrl);

            if (!imageResponse.IsSuccessStatusCode)
                return null;

            var imageJson = await imageResponse.Content.ReadAsStringAsync();
            var imageData = JsonSerializer.Deserialize<ResponseWrapper<string>>(imageJson);
            var base64Image = imageData?.Body;

            if (string.IsNullOrWhiteSpace(base64Image))
                return null;

            byte[] imageBytes = Convert.FromBase64String(base64Image);
            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
        catch
        {
            return null;
        }
    }

    private async Task LoadUserProducts()
    {
        try
        {
            var username = UserSession.Instance.Username;
            var token = UserSession.Instance.JwtToken;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
            {
                await DisplayAlert("Error", "No estas autorizado para ver esto", "OK");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(UserEndpoints.GetMyProducts(username));

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "No se pudieron cargar tus productos...", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var productsResponse = JsonSerializer.Deserialize<ProductoListResponse>(json);

            if (productsResponse?.Body == null)
            {
                await DisplayAlert("Error", "Respuesta inválida del servidor", "OK");
                return;
            }

            var productos = new List<ProductoViewModel>();

            foreach (var p in productsResponse.Body)
            {
                var imageSource = await CargarImagenProductoAsync(p.Id);

                productos.Add(new ProductoViewModel
                {
                    Id = p.Id,
                    Nombre = p.Name,
                    Precio = p.Price,
                    CantidadVendidos = p.NumberSold ?? 0,
                    ImageSource = imageSource,
                    EsPropio = true,
                    Estado = p.Status
                });
            }

            _allProducts = productos; 
            ItemsCollection.ItemsSource = productos;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Excepcion", ex.Message, "OK");
        }
    }

    private async Task LoadUserAuctions()
    {
        try
        {
            var username = UserSession.Instance.Username;
            var token = UserSession.Instance.JwtToken;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
            {
                await DisplayAlert("Error", "Sesión no válida.", "OK");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = AuctionEndpoints.GetMyAuctions(username);

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "No se pudieron obtener las subastas.", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var wrapper = JsonSerializer.Deserialize<ResponseWrapper<List<MyAuctionsDTO>>>(json);

            if (wrapper?.Body == null || wrapper.Body.Count == 0)
            {
                await DisplayAlert("Aviso", "No hay subastas creadas.", "OK");
                return;
            }

            foreach (var auction in wrapper.Body)
            {
                if (!string.IsNullOrWhiteSpace(auction.ImageBase64))
                {
                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(auction.ImageBase64);
                        auction.ImageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    }
                    catch
                    {
                        auction.ImageSource = null;
                    }
                }
            }

            AuctionsCollection.ItemsSource = wrapper.Body;

            _allAuctions = wrapper.Body;

            InitializeAuctionStatusPicker();
            AuctionStatusPicker.SelectedIndex = 0;

            _AuctionsLoaded = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Excepción: {ex.Message}", "OK");
        }
    }

    private void BtnLoadUserProducts(object sender, EventArgs e)
    {
        ItemsCollection.IsVisible = true;
        AuctionsCollection.IsVisible = false;
        if (_productsLoaded) return;

        LoadUserProducts();
        _productsLoaded = true;
    }

    private void BtnLoadUserAuctions(object sender, EventArgs e)
    {
        ItemsCollection.IsVisible = false;
        AuctionsCollection.IsVisible = true;

        if (_AuctionsLoaded) return;
        LoadUserAuctions();
        _AuctionsLoaded = true;
    }

    private void OnAuctionStatusChanged(object sender, EventArgs e)
    {
        var selectedStatus = AuctionStatusPicker.SelectedItem?.ToString();

        if (string.IsNullOrWhiteSpace(selectedStatus) || selectedStatus == "Todos")
        {
            AuctionsCollection.ItemsSource = _allAuctions;
            return;
        }

        var filtered = _allAuctions
            .Where(a => a.Status.Equals(selectedStatus, StringComparison.OrdinalIgnoreCase))
            .ToList();

        AuctionsCollection.ItemsSource = filtered;
    }

    private async void BtnNavigateToStatsPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserStatsPage());
    }

    private async void BtnEndAuction(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is MyAuctionsDTO auction)
        {
            bool confirm = await DisplayAlert("Confirmar", $"¿Deseas eliminar la subasta '{auction.Name}'?", "Sí", "No");
            if (!confirm) return;

            var token = UserSession.Instance.JwtToken;
            if (string.IsNullOrWhiteSpace(token))
            {
                await DisplayAlert("Error", "Token inválido.", "OK");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var payload = new UpdateAuctionDTO
            {
                AuctionId = auction.Id,
                StatusId = 4
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, AuctionEndpoints.UpdateAuction)
            {
                Content = JsonContent.Create(payload)
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                _allAuctions.Remove(auction);
                AuctionsCollection.ItemsSource = null;
                AuctionsCollection.ItemsSource = _allAuctions;

                await DisplayAlert("Éxito", "Subasta finalizada correctamente.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo finalizar la subasta.", "OK");
            }
        }

    }

    private async void BtnAuctionDetails(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is MyAuctionsDTO auction)
        {
            await Navigation.PushAsync(new AuctionDetailsPage(auction.Id));
        }
    }

}