namespace ClienteMAUI.Views;

using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Pruducts;
using ClienteMAUI.Models.ViewModel;
using ClienteMAUI.Session;
using ClienteMAUI.Views;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public partial class MainMenuPage : ContentPage
{
    private readonly HttpClient _httpClient = new();
    private List<TypeDTO> typesList = new();

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
        _ = CargarTiposAsync();
        _ = CargarProductosAsync();
    }

    private async Task CargarTiposAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(ProductEndpoints.GetTypes);
            if (!response.IsSuccessStatusCode) return;

            var json = await response.Content.ReadAsStringAsync();
            var typesResponse = JsonSerializer.Deserialize<ResponseWrapper<List<TypeDTO>>>(json);
            if (typesResponse?.Body != null)
            {
                typesList = typesResponse.Body; 
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar tipos:\n{ex.Message}", "OK");
        }
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

    private async void OnModificarProductoClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is ProductoViewModel producto)
        {
            try
            {
                var token = UserSession.Instance.JwtToken;
                if (string.IsNullOrWhiteSpace(token))
                {
                    await DisplayAlert("Error", "Sesión inválida", "OK");
                    return;
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var url = ProductEndpoints.GetProductDetails(producto.Id);
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Error", "No se pudo obtener el producto", "OK");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();

                // Deserializa el DTO
                var dtoResponse = JsonSerializer.Deserialize<ResponseWrapper<ProductDTO>>(json);

                

                if (dtoResponse?.Body == null)
                {
                    await DisplayAlert("Error", "Producto no válido", "OK");
                    return;
                }               

                // Navega con el DTO completo
                await Navigation.PushAsync(new ProductFormPage(dtoResponse.Body));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Excepción", ex.Message, "OK");
            }
        }
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

            var productos = new List<ProductoViewModel>();

            foreach (var p in productosResponse.Body)
            {
                var imageSource = await CargarImagenProductoAsync(p.Id);

                productos.Add(new ProductoViewModel
                {
                    Id = p.Id,
                    Nombre = p.Name,
                    Precio = p.Price,
                    CantidadVendidos = p.NumberSold ?? 0,
                    ImageSource = imageSource,
                    EsPropio = false
                });
            }

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

            var productos = new List<ProductoViewModel>();

            foreach (var p in productosResponse.Body)
            {
                var image = await CargarImagenProductoAsync(p.Id);

                productos.Add(new ProductoViewModel
                {
                    Id = p.Id,
                    Nombre = p.Name,
                    Precio = p.Price,
                    CantidadVendidos = p.NumberSold ?? 0,
                    ImageSource = image,
                    EsPropio = nombreCategoria.Equals("Mis productos", StringComparison.OrdinalIgnoreCase)
                });
            }

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

    private async Task<ImageSource?> CargarImagenProductoAsync(int productId)
    {
        try
        {
            var imageUrl = ProductEndpoints.GetProductImage(productId);
            var imageResponse = await _httpClient.GetAsync(imageUrl);

            if (!imageResponse.IsSuccessStatusCode)
                return null;

            var imageJson = await imageResponse.Content.ReadAsStringAsync();
            var imageData = JsonSerializer.Deserialize<ResponseWrapper<string>>(imageJson);
            var base64Image = imageData?.Body;

            if (string.IsNullOrWhiteSpace(base64Image))
                return null;

            byte[] imageBytes = Convert.FromBase64String(base64Image);
            return ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
        catch
        {
            return null; 
        }
    }

    private async void OnVerSubastasClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AuctionsMenuPage());
    }

    private async void NavigateToProfilePage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserProfilePage());
    }
}