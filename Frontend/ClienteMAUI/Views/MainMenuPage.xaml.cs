namespace ClienteMAUI.Views;

using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Pruducts;
using ClienteMAUI.Models.ViewModel;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public partial class MainMenuPage : ContentPage
{
    private readonly HttpClient _httpClient = new();


    public MainMenuPage()
	{
		InitializeComponent();
        _httpClient = new HttpClient();
        _ = CargarCategoriasAsync();
        _ = CargarProductosAsync();
    }
    


    private void OnCrearProductoClicked(object sender, EventArgs e)
    {

    }

    private void OnEliminarProductoClicked(object sender, EventArgs e)
    {

    }

    private void OnModificarProductoClicked(object sender, EventArgs e)
    {

    }
    private async Task CargarCategoriasAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(ProductEndpoints.GetCategories);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

            
                var categoriaResponse = JsonSerializer.Deserialize<CategoriaResponse>(json);

                if (categoriaResponse?.Body != null)
                {
                
                    var categorias = categoriaResponse.Body;

                    CategoryButtonsLayout.Children.Clear();
                    CategoryButtonsLayout.Children.Add(CrearBotonCategoria("Todos"));
                    CategoryButtonsLayout.Children.Add(CrearBotonCategoria("Mis productos"));

                    foreach (var cat in categorias)
                    {
                        CategoryButtonsLayout.Children.Add(CrearBotonCategoria(cat.Category));
                    }

                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar categorías:\n{ex.GetType().Name}\n{ex.Message}", "OK");
        }
    }

    private async Task CargarProductosAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(ProductEndpoints.GetProducts);

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "No se pudo obtener la lista de productos", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var productosResponse = JsonSerializer.Deserialize<ProductoListResponse>(json);

            if (productosResponse?.Body == null)
            {
                await DisplayAlert("Error", "Respuesta inválida del servidor", "OK");
                return;
            }

            var productos = productosResponse.Body.Select(p => new ProductoViewModel
            {
                Nombre = p.Name,
                Precio = p.Price,
                CantidadVendidos = p.NumberSold,
                ImageSource = null //imagen vendrá después por gRPC
            }).ToList();

            ProductsCollection.ItemsSource = productos;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Excepción", ex.Message, "OK");
        }
    }


    private Button CrearBotonCategoria(string texto)
    {
        return new Button
        {
            Text = texto,
            Padding = 10,
            Margin = 5,
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black,
            CornerRadius = 12,
            Command = new Command(async () => await FiltrarPorCategoria(texto))
        };
    }


    private async Task FiltrarPorCategoria(string nombreCategoria)
    {
        try
        {
            var url = $"{ProductEndpoints.GetProducts}?query={Uri.EscapeDataString(nombreCategoria)}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", "No se pudo obtener la lista de productos", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var productosResponse = JsonSerializer.Deserialize<ProductoListResponse>(json);

            if (productosResponse?.Body == null)
            {
                await DisplayAlert("Error", "Respuesta inválida del servidor", "OK");
                return;
            }

            var productos = productosResponse.Body.Select(p => new ProductoViewModel
            {
                Nombre = p.Name,
                Precio = p.Price,
                CantidadVendidos = p.NumberSold,
                ImageSource = null
            }).ToList();

            ProductsCollection.ItemsSource = productos;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Excepción", ex.Message, "OK");
        }
    }


    public class CategoriaResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("body")]
        public List<CategoriaViewModel> Body { get; set; } = new();
    }


}