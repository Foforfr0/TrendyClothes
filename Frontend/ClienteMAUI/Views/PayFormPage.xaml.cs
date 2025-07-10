using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Auctions;
using ClienteMAUI.Session;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ClienteMAUI.Views;

public partial class PayFormPage : ContentPage
{
    private AuctionsListDTO _auction;

    public PayFormPage(AuctionsListDTO auction)
    {
        InitializeComponent();
        _auction = auction;

        ProductNameLabel.Text = _auction.ProductName;
        ProductPriceLabel.Text = $"Total a pagar: ${_auction.LastPrice:N2}";

        if (!string.IsNullOrEmpty(_auction.ImageBase64))
        {
            ProductImage.Source = ImageSource.FromStream(() =>
                new MemoryStream(Convert.FromBase64String(_auction.ImageBase64)));
        }
    }


    private async void OnConfirmPurchaseClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
            string.IsNullOrWhiteSpace(AddressEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PhoneEntry.Text) ||
            string.IsNullOrWhiteSpace(CardNumberEntry.Text) ||
            string.IsNullOrWhiteSpace(ExpirationEntry.Text) ||
            string.IsNullOrWhiteSpace(CVVEntry.Text))
        {
            await DisplayAlert("Error", "Por favor llena todos los campos para completar el pago.", "OK");
            return;
        }
        try
        {
            var token = UserSession.Instance.JwtToken;
            if (string.IsNullOrWhiteSpace(token))
            {
                await DisplayAlert("Error", "Token inválido.", "OK");
                return;
            }

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var payload = new UpdateAuctionDTO
            {
                AuctionId = _auction.Id,
                StatusId = 5 // Estado "Pagada"
            };

            var request = new HttpRequestMessage(HttpMethod.Patch, AuctionEndpoints.UpdateAuction)
            {
                Content = JsonContent.Create(payload)
            };

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Pago exitoso", $"Has pagado ${_auction.LastPrice:N2} por la subasta '{_auction.ProductName}'", "OK");
                await Navigation.PopAsync(); // Volver a la página anterior
            }
            else
            {
                var errorText = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"No se pudo actualizar el estado de la subasta: {errorText}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error durante el pago: {ex.Message}", "OK");
        }
    }
}
