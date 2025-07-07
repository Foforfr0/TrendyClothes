using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Auctions;
using ClienteMAUI.Session;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
            var response = await _httpClient.GetAsync(AuctionEndpoints.GetAuctions);
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
        if (sender is Button btn && btn.CommandParameter is int auctionId)
        {
            var username = UserSession.Instance.Username;
            var jwtToken = UserSession.Instance.JwtToken;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(jwtToken))
            {
                await DisplayAlert("Error", "Sesión no válida", "OK");
                return;
            }

            // Obtener el objeto actual del binding
            if (btn.BindingContext is not AuctionsListDTO localAuction)
            {
                await DisplayAlert("Error", "No se pudo obtener información de la subasta.", "OK");
                return;
            }

            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwtToken);

                // Obtener subasta actualizada
                var response = await httpClient.GetAsync(AuctionEndpoints.GetAuctionDetails(auctionId));

                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Error", "No se pudo verificar el precio actual.", "OK");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var auctionUpdated = JsonSerializer.Deserialize<ResponseWrapper<AuctionsListDTO>>(json)?.Body;

                if (auctionUpdated == null)
                {
                    await DisplayAlert("Error", "No se pudo obtener la subasta actualizada.", "OK");
                    return;
                }

                // Verificar si ha cambiado el precio
                if (auctionUpdated.LastPrice != localAuction.LastPrice)
                {
                    await DisplayAlert("Puja actualizada",
                        $"El precio actual ha cambiado. Último precio: ${auctionUpdated.LastPrice}",
                        "Aceptar");

                    await CargarSubastasAsync();
                    return;
                }

                // Aumentar el precio de la puja
                var increaseResponse = await httpClient.PutAsync(AuctionEndpoints.IncreaseBid(auctionId), null);

                if (!increaseResponse.IsSuccessStatusCode)
                {
                    var errorText = await increaseResponse.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo aumentar el precio: {errorText}", "OK");
                    return;
                }

                // Registrar la puja
                var bidPayload = new
                {
                    AuctionId = auctionId,
                    Username = username
                };

                var bidContent = JsonContent.Create(bidPayload);
                var registerResponse = await httpClient.PostAsync(AuctionEndpoints.RegisterBid, bidContent);

                if (registerResponse.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "¡Puja registrada correctamente!", "OK");
                }
                else
                {
                    var error = await registerResponse.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo registrar la puja: {error}", "OK");
                }

                await CargarSubastasAsync(); // recarga la vista al final
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
    }

    


    private async void OnMisSubastasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MyAuctionsMenuPage());
    }

    private async void OnCrearSubastaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AuctionFormPage());
    }
}
