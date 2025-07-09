using ClienteMAUI.Models.DTO.Auctions;

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

        await DisplayAlert("Pago exitoso", $"Has pagado ${_auction.LastPrice:N2} por la subasta '{_auction.ProductName}'", "OK");

        // Aquí se llamara al backend para cambiar el estado de la subasta a pagada
    }
}
