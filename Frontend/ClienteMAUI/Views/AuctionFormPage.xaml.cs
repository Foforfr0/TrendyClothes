using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Auctions;
using ClienteMAUI.Session;
using System.Text.Json;


namespace ClienteMAUI.Views;

public partial class AuctionFormPage : ContentPage
{
    private string? imageBase64;
    private string? mimeImage;

    public AuctionFormPage()
    {
        InitializeComponent();
    }

    private async void OnSelectImageClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Seleccionar imagen",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            var stream = await result.OpenReadAsync();
            var bytes = new BinaryReader(stream).ReadBytes((int)stream.Length);
            imageBase64 = Convert.ToBase64String(bytes);
            mimeImage = result.ContentType;
            imgPreview.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
        }
    }

    private async void OnCreateAuctionClicked(object sender, EventArgs e)
    {
        if (!ValidarFormulario(out var error))
        {
            await DisplayAlert("Validación", error, "OK");
            return;
        }

        // Convertir campos seguros
        decimal firstPrice = decimal.Parse(entryFirstPrice.Text);
        decimal bid = decimal.Parse(entryBid.Text);
        DateTime start = dateStartPicker.Date + timeStartPicker.Time;
        DateTime end = dateEndPicker.Date + timeEndPicker.Time;

        var auction = new CreateAuctionDTO
        {
            Name = entryName.Text,
            FirstPrice = firstPrice,
            Bid = bid,
            DateStart = start,
            DateEnd = end,
            Description = editorDescription.Text,
            StatusId = pickerStatus.SelectedIndex == 0 ? 1 : 2,
            SellerUsername = UserSession.Instance.Username,
            imageBase64 = imageBase64,
            mimeImage = mimeImage
        };

        var json = JsonSerializer.Serialize(auction);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserSession.Instance.JwtToken);

        var response = await httpClient.PostAsync(AuctionEndpoints.CreateAuction, content);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Éxito", "Subasta creada correctamente", "OK");
            LimpiarFormulario();
            await Navigation.PushAsync(new AuctionsMenuPage());
        }
        else
        {
            await DisplayAlert("Error", "No se pudo crear la subasta", "OK");
        }
    }

    private bool ValidarFormulario(out string mensajeError)
    {
        mensajeError = string.Empty;

        if (string.IsNullOrWhiteSpace(entryName.Text) ||
            string.IsNullOrWhiteSpace(entryFirstPrice.Text) ||
            string.IsNullOrWhiteSpace(entryBid.Text) ||
            string.IsNullOrWhiteSpace(editorDescription.Text) ||
            pickerStatus.SelectedIndex == -1 ||
            imageBase64 == null ||
            mimeImage == null)
        {
            mensajeError = "Por favor, llena todos los campos y selecciona una imagen.";
            return false;
        }

        if (!decimal.TryParse(entryFirstPrice.Text, out var fp) || fp < 0)
        {
            mensajeError = "El precio inicial debe ser un número positivo.";
            return false;
        }

        if (!decimal.TryParse(entryBid.Text, out var bid) || bid <= 0)
        {
            mensajeError = "La puja mínima debe ser un número mayor a 0.";
            return false;
        }

        var start = dateStartPicker.Date + timeStartPicker.Time;
        var end = dateEndPicker.Date + timeEndPicker.Time;

        if (start >= end)
        {
            mensajeError = "La fecha de fin debe ser posterior a la fecha de inicio.";
            return false;
        }

        if (editorDescription.Text.Length < 10 || editorDescription.Text.Length > 500)
        {
            mensajeError = "La descripción debe tener entre 10 y 500 caracteres.";
            return false;
        }

        return true;
    }

    private void LimpiarFormulario()
    {
        entryName.Text = string.Empty;
        entryFirstPrice.Text = string.Empty;
        entryBid.Text = string.Empty;
        editorDescription.Text = string.Empty;
        pickerStatus.SelectedIndex = -1;
        dateStartPicker.Date = DateTime.Today;
        timeStartPicker.Time = TimeSpan.Zero;
        dateEndPicker.Date = DateTime.Today;
        timeEndPicker.Time = TimeSpan.Zero;
        imgPreview.Source = null;
        imageBase64 = null;
        mimeImage = null;
    }
}
