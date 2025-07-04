using ClienteMAUI.Connections;
using ClienteMAUI.Models.DTO.Pruducts;
using ClienteMAUI.Session;
using Microsoft.Maui.Controls;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClienteMAUI.Views;

public partial class ProductFormPage : ContentPage
{
    private readonly HttpClient _httpClient = new();
    private ProductDTO _product;

    public ProductFormPage(ProductDTO? product = null)
    {
        InitializeComponent();
        _product = product ?? new ProductDTO(); // Si viene null, es creación
        CargarTagsAsync();
        CargarDatosFormulario();
    }

    private async void CargarTagsAsync()
    {
        try
        {
            // Categorías
            var catResponse = await _httpClient.GetFromJsonAsync<ResponseWrapper<List<CategoryDTO>>>(ProductEndpoints.GetCategories);
            pickerCategory.ItemsSource = catResponse?.Body ?? new();
            pickerCategory.ItemDisplayBinding = new Binding("Category");

            // Tipos
            var typeResponse = await _httpClient.GetFromJsonAsync<ResponseWrapper<List<TypeDTO>>>(ProductEndpoints.GetTypes);
            pickerType.ItemsSource = typeResponse?.Body ?? new();
            pickerType.ItemDisplayBinding = new Binding("Type");

            // Estados
            var statusResponse = await _httpClient.GetFromJsonAsync<ResponseWrapper<List<StatusDTO>>>(ProductEndpoints.GetStatuses);
            pickerStatus.ItemsSource = statusResponse?.Body ?? new();
            pickerStatus.ItemDisplayBinding = new Binding("Status");

           
                // Si estamos editando, seleccionamos los valores actuales
            if (_product.Id != null)
            {
                pickerCategory.SelectedItem = ((List<CategoryDTO>)pickerCategory.ItemsSource)
                    .FirstOrDefault(c => c.Category.Equals(_product.CategoryName, StringComparison.OrdinalIgnoreCase));

                pickerType.SelectedItem = ((List<TypeDTO>)pickerType.ItemsSource)
                    .FirstOrDefault(t => t.Type.Equals(_product.TypeName, StringComparison.OrdinalIgnoreCase));

                pickerStatus.SelectedItem = ((List<StatusDTO>)pickerStatus.ItemsSource)
                    .FirstOrDefault(s => s.Id == _product.StatusId);

                CargarDatosFormulario(); 
            }

            
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudieron cargar los datos del formulario: " + ex.Message, "OK");
        }
    }


    private void CargarDatosFormulario()
    {
        if (_product.Id == null) return;

        txtName.Text = _product.Name;
        txtPrice.Text = _product.Price.ToString();
        txtDiscount.Text = _product.Discount.ToString();
        txtStock.Text = _product.StockAvailable.ToString();
        txtDescription.Text = _product.Description;
        

    }

    private async void OnGuardarProductoClicked(object sender, EventArgs e)
    {
        try
        {
            var selectedCategory = (CategoryDTO)pickerCategory.SelectedItem;
            var selectedType = (TypeDTO)pickerType.SelectedItem;
            var selectedStatus = (StatusDTO)pickerStatus.SelectedItem;

            _product.Name = txtName.Text;
            _product.Price = decimal.Parse(txtPrice.Text);
            _product.Discount = float.Parse(txtDiscount.Text);
            _product.StockAvailable = int.Parse(txtStock.Text);
            _product.Description = txtDescription.Text;
            _product.CategoryId = selectedCategory.Id;
            _product.TypeId = selectedType.Id;
            _product.StatusId = selectedStatus.Id;
            _product.UsernameSeller = Preferences.Get("username", "anonimo");

            var token = UserSession.Instance.JwtToken;
            if (string.IsNullOrWhiteSpace(token))
            {
                await DisplayAlert("Error", "Sesión no válida. No se encontró el token.", "OK");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response;

            if (_product.Id == null)
            {
                var jsonDebug = JsonSerializer.Serialize(_product);
                Console.WriteLine($"[DEBUG] JSON enviado: {jsonDebug}");
                // CREAR
                response = await _httpClient.PostAsJsonAsync(UserEndpoints.CreateProduct, _product);
            }
            else
            {
                // EDITAR
                response = await _httpClient.PutAsJsonAsync(UserEndpoints.UpdateProduct, _product);

            }

            if (response.IsSuccessStatusCode)
                await DisplayAlert("Éxito", "Producto guardado correctamente", "OK");
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"No se pudo guardar el producto:\n{error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Ocurrió un error: " + ex.Message, "OK");
        }
    }

}

// Clase genérica para manejar la respuesta JSON del backend
public class ResponseWrapper<T>
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("body")]
    public T? Body { get; set; }  
}
