namespace ClienteMAUI.Views;

using ClienteMAUI.Connections;
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

                    CategoryButtonsLayout.Children.Add(CrearBotonCategoria("Todos", 0));
                    CategoryButtonsLayout.Children.Add(CrearBotonCategoria("Mis productos", -1));

                    foreach (var cat in categorias)
                    {
                        Console.WriteLine($"CATEGORIA -> Id: {cat.Id}, Nombre: {cat.Category}");
                        CategoryButtonsLayout.Children.Add(CrearBotonCategoria(cat.Category, cat.Id));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar categorías:\n{ex.GetType().Name}\n{ex.Message}", "OK");
        }
    }

    private Button CrearBotonCategoria(string texto, int categoriaId)
    {
        return new Button
        {
            Text = texto,
            Padding = 10,
            Margin = 5,
            BackgroundColor = Colors.LightGray,
            TextColor = Colors.Black,
            CornerRadius = 12,
            Command = new Command(async () => await FiltrarPorCategoria(categoriaId))
        };
    }

    private async Task FiltrarPorCategoria(int categoriaId)
    {
        await DisplayAlert("Categoría seleccionada", $"ID categoría: {categoriaId}", "OK");
    }

    public class CategoriaResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("body")]
        public List<CategoriaViewModel> Body { get; set; } = new();
    }


}