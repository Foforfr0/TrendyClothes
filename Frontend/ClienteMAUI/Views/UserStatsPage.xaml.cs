using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Auctions;
using ClienteMAUI.Models.DTO.Pruducts;
using ClienteMAUI.Models.ViewModel;
using ClienteMAUI.Session;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ClienteMAUI.Views;

public partial class UserStatsPage : ContentPage
{
    private readonly HttpClient _httpClient = new();

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadUserStatsAsync();
        await LoadUserProductsAsync();
    }

    public UserStatsPage()
	{
		InitializeComponent();
	}

    private async Task LoadUserStatsAsync()
    {
        try
        {
            var username = UserSession.Instance.Username;
            var token = UserSession.Instance.JwtToken;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
            {
                await DisplayAlert("Error", "Usuario no autenticado.", "OK");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(AuctionEndpoints.GetMyAuctions(username));

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "No se pudieron cargar las subastas.", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var auctions = JsonSerializer.Deserialize<ResponseWrapper<List<MyAuctionsDTO>>>(json)?.Body;

            if (auctions == null)
            {
                await DisplayAlert("Error", "No se encontraron subastas.", "OK");
                return;
            }

            
            LbTotalAuctions.Text = auctions.Count.ToString();

            int totalBids = auctions.Sum(a => a.BidsCount ?? 0);
            LbTotalBids.Text = totalBids.ToString();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar estadísticas: {ex.Message}", "OK");
        }
    }

    private async Task LoadUserProductsAsync()
    {
        try
        {
            var username = UserSession.Instance.Username;
            var token = UserSession.Instance.JwtToken;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
            {
                await DisplayAlert("Error", "No estás autorizado para ver esto", "OK");
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
                productos.Add(new ProductoViewModel
                {
                    Id = p.Id,
                    Nombre = p.Name,
                    Precio = p.Price,
                    CantidadVendidos = p.NumberSold ?? 0,
                    Estado = p.Status
                });
            }

            LbTotalProducts.Text = productos.Count.ToString();

            var totalSales = productos.Sum(p => p.Precio * p.CantidadVendidos);
            LbTotalSales.Text = $"${totalSales:N2}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Excepción", ex.Message, "OK");
        }
    }

}