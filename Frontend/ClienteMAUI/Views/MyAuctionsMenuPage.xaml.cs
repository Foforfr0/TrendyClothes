namespace ClienteMAUI.Views;

using ClienteMAUI.Models.DTO.Auctions;
using ClienteMAUI.Session;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
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
            var url = $"http://10.0.2.2:5000/api/Auction/Auctioneer/ConsultMyAuctions/MyAuctions?username={Uri.EscapeDataString(username)}";

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
            await DisplayAlert("Eliminar", $"Eliminar subasta con ID: {auction.Id}", "OK");
            // Aquí se conectará el endpoint de eliminación luego
        }
    }
}
