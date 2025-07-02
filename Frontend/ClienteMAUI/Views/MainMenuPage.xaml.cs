namespace ClienteMAUI.Views;

using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Pruducts;
using ClienteMAUI.Models.ViewModel;
using ClienteMAUI.Session;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClienteMAUI.Views;


public partial class MainMenuPage : ContentPage
{
    private readonly HttpClient _httpClient = new();


    public MainMenuPage()
	{
		InitializeComponent();
        var savedUsername = Preferences.Get("username", null);
        var savedJwt = Preferences.Get("jwtToken", null);

        if (!string.IsNullOrWhiteSpace(savedUsername) && !string.IsNullOrWhiteSpace(savedJwt))
        {
            UserSession.Instance.SetUser(savedUsername, savedJwt);
        }
        Console.WriteLine($"[MainMenu] Username: {savedUsername}");
        Console.WriteLine($"[MainMenu] JWT: {savedJwt}");
        Console.WriteLine($"[UserSession] Username: {UserSession.Instance.Username}");

        _ = CargarCategoriasAsync();
        _ = CargarProductosAsync();
    }
    


    private async void OnCrearProductoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductFormPage());
    }

    private async void OnEliminarProductoClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is ProductoViewModel producto)
        {
            bool confirm = await DisplayAlert("Confirmar", $"¿Eliminar el producto '{producto.Nombre}'?", "Sí", "No");
            if (!confirm) return;

            try
            {
                var token = UserSession.Instance.JwtToken;

                if (string.IsNullOrWhiteSpace(token))
                {
                    await DisplayAlert("Error", "Sesión no válida. No hay token disponible.", "OK");
                    return;
                }

                // Establecer el JWT en la cabecera
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var url = UserEndpoints.DeleteProduct(producto.Id);
                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                var response = await _httpClient.SendAsync(request);


                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Producto eliminado correctamente", "OK");
                    await FiltrarPorCategoria("Mis productos");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo eliminar el producto: {error}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Excepción", ex.Message, "OK");
            }
        }
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
                ImageSource = null, //imagen vendrá después por gRPC
                EsPropio = false
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
            string url;

            if (nombreCategoria.Equals("Todos", StringComparison.OrdinalIgnoreCase))
            {
                // Mostrar todos los productos
                url = ProductEndpoints.GetProducts;
            }
            else if (nombreCategoria.Equals("Mis productos", StringComparison.OrdinalIgnoreCase))
            {
                // Verificar si hay sesión
                var username = UserSession.Instance.Username;
                var token = UserSession.Instance.JwtToken;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
                {
                    await DisplayAlert("Error", "No se ha iniciado sesión correctamente.", "OK");
                    return;
                }

                // Establecer token en headers
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Endpoint para mis productos
                url = UserEndpoints.GetMyProducts(username);
            }
            else
            {
                // Filtro por categoría
                url = $"{ProductEndpoints.GetProducts}?query={Uri.EscapeDataString(nombreCategoria)}";
            }

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
                Id = p.Id,
                Nombre = p.Name,
                Precio = p.Price,
                CantidadVendidos = p.NumberSold,
                ImageSource = null,
                EsPropio = nombreCategoria.Equals("Mis productos", StringComparison.OrdinalIgnoreCase)
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