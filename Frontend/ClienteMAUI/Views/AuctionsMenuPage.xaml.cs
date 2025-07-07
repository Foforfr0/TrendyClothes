using ClienteMAUI.Connections;
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
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            
        }
    }

    private void OnMisSubastasClicked(object sender, EventArgs e)
    {

    }

    private async void OnCrearSubastaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AuctionFormPage());
    }
}
