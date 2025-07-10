namespace ClienteMAUI.Views;

using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Auctions;
using ClienteMAUI.Session;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;


public partial class MyAuctionsMenuPage : ContentPage
{
    public ObservableCollection<MyAuctionsDTO> Auctions { get; set; } = new();
    private readonly HttpClient _httpClient = new();

    public MyAuctionsMenuPage()
    {
        InitializeComponent();
        BindingContext = this;
        _ = CargarMisSubastasAsync();
    }

    private async Task CargarMisSubastasAsync()
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

            Auctions.Clear();
            foreach (var auction in wrapper.Body)
            {
                byte[] imageBytes = Convert.FromBase64String(auction.ImageBase64);
                auction.ImageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                Auctions.Add(auction);
            }

            var ganancias = wrapper.Body
                .Where(a => a.Status.Trim().ToLower().Contains("pagad"))
                .Sum(a => a.LastPrice);

            lblGanancias.Text = $"Ganancias totales: ${ganancias:N2}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Excepción: {ex.Message}", "OK");
        }
    }


    private async void OnEliminarSubastaClicked(object sender, EventArgs e)
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
                StatusId = 2
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, AuctionEndpoints.UpdateAuction)
            {
                Content = JsonContent.Create(payload)
            };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Auctions.Remove(auction);
                await DisplayAlert("Éxito", "Subasta eliminada correctamente.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo eliminar la subasta.", "OK");
            }
        }
    }

    private async void OnFinalizarSubastaClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is MyAuctionsDTO auction)
        {
            bool confirm = await DisplayAlert("Confirmar", $"¿Deseas finalizar la subasta '{auction.Name}' con '{auction.LastPriceText}'?", "Sí", "No");
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
                Auctions.Remove(auction);
                await DisplayAlert("Éxito", $"Has finalizado la subasta con ${auction.LastPrice:N2}", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo finalizar la subasta.", "OK");
            }
        }
    }
}
