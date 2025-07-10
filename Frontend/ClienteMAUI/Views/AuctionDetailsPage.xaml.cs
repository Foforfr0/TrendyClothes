using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Auctions;
using ClienteMAUI.Session;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ClienteMAUI.Views;

public partial class AuctionDetailsPage : ContentPage
{
	private readonly HttpClient _httpClient = new();
	private readonly int _auctionId;
    public ObservableCollection<BidHistoryEntry> BidHistory { get; set; } = new();

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var history = BidHistoryStore.GetBids(_auctionId);
        BidHistory.Clear();

        foreach (var bid in history)
            BidHistory.Add(bid);

        BidsCollectionView.ItemsSource = BidHistory;
    }

    public AuctionDetailsPage(int auctionId)
	{
		InitializeComponent();
		_auctionId = auctionId;
		LoadAuctionDetailsAsync();
	}

    private async void LoadAuctionDetailsAsync()
    {
        try
        {
            var token = UserSession.Instance.JwtToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(AuctionEndpoints.GetAuctionDetails(_auctionId));
            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "No se pudo cargar la subasta.", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var wrapper = JsonSerializer.Deserialize<ResponseWrapper<AuctionsListDTO>>(json);

            var auction = wrapper?.Body;
            if (auction == null)
            {
                await DisplayAlert("Error", "Datos de subasta no válidos.", "OK");
                return;
            }

            LbName.Text = auction.ProductName;
            LbStartPrice.Text = $"Precio inicial: ${auction.FirstPrice:N2}";
            LbLastPrice.Text = $"Última puja: ${auction.LastPrice:N2}";
            LbBidIncrement.Text = $"Incremento: ${auction.Bid:N2}";
            LblDateEnd.Text = $"Fin: {auction.DateEnd:dd/MM/yyyy HH:mm}";

            if (!string.IsNullOrEmpty(auction.ImageBase64))
            {
                byte[] imageBytes = Convert.FromBase64String(auction.ImageBase64);
                AuctionImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Excepción: {ex.Message}", "OK");
        }
    }
}