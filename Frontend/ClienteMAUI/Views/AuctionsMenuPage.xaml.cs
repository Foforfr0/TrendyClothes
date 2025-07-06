using ClienteMAUI.Models.DTO.Auctions;
using ClienteMAUI.Session;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ClienteMAUI.Views;

public partial class AuctionsMenuPage : ContentPage
{
    private readonly HttpClient _httpClient = new();

    public AuctionsMenuPage()
    {
        InitializeComponent();
        _ = CargarSubastasAsync();
    }

    private async Task CargarSubastasAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://10.0.2.2:5000/api/Auctions/Auction");
            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "No se pudo obtener la lista de subastas.", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ResponseWrapper<List<AuctionsListDTO>>>(json);

            if (result?.Body == null || result.Body.Count == 0)
            {
                lblNoAuctions.IsVisible = true;
                return;
            }

            foreach (var auction in result.Body)
            {
                auction.FirstPriceText = $"Precio inicial: ${auction.FirstPrice}";
                auction.LastPriceText = $"Última puja: ${auction.LastPrice}";
                auction.BidText = $"Puja mínima: ${auction.Bid}";
                auction.EndDateFormatted = $"Termina: {auction.DateEnd:dd/MM/yyyy HH:mm}";

                if (!string.IsNullOrWhiteSpace(auction.ImageBase64))
                {
                    var bytes = Convert.FromBase64String(auction.ImageBase64);
                    auction.ImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                }
            }

            AuctionsCollection.ItemsSource = result.Body;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Excepción", ex.Message, "OK");
        }
    }

    private async void OnPujarClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            //await Navigation.PushAsync(new AuctionDetailsPage(id));
        }
    }
    private async Task PujarAsync(int auctionId, decimal shownLastPrice)
    {
        try
        {
            var token = UserSession.Instance.JwtToken;
            var buyerId = UserSession.Instance.;

            if (string.IsNullOrWhiteSpace(token) || buyerId == 0)
            {
                await DisplayAlert("Error", "No hay sesión válida", "OK");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // 1. Verificar precio actual
            var getResponse = await _httpClient.GetAsync($"http://10.0.2.2:5000/api/Auctions/Auction/ById/{auctionId}");
            if (!getResponse.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "No se pudo verificar el precio actual", "OK");
                return;
            }

            var getJson = await getResponse.Content.ReadAsStringAsync();
            var auctionData = JsonSerializer.Deserialize<ResponseWrapper<AuctionsListDTO>>(getJson);

            if (auctionData?.Body == null)
            {
                await DisplayAlert("Error", "La subasta no fue encontrada", "OK");
                return;
            }

            var currentAuction = auctionData.Body;

            if (currentAuction.LastPrice != shownLastPrice)
            {
                await DisplayAlert("Atención", "La puja ha cambiado desde la última vez. Actualiza para ver el nuevo precio.", "OK");
                return;
            }

            // 2. Incrementar el precio
            var increaseResponse = await _httpClient.PutAsync(
                $"http://10.0.2.2:5000/api/Auctions/Auction/IncreaseBid/{auctionId}", null);

            if (!increaseResponse.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "No se pudo incrementar el precio", "OK");
                return;
            }

            // 3. Registrar la puja
            var payload = new
            {
                AuctionId = auctionId,
                BuyerId = buyerId
            };

            var bidContent = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var registerResponse = await _httpClient.PostAsync("http://10.0.2.2:5000/api/Auctions/Auction/RegisterBid", bidContent);

            if (!registerResponse.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "No se pudo registrar la puja", "OK");
                return;
            }

            await DisplayAlert("¡Éxito!", "¡Puja registrada exitosamente!", "OK");

            // Opcional: recargar subastas
            await CargarSubastasAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al pujar: {ex.Message}", "OK");
        }
    }

}
