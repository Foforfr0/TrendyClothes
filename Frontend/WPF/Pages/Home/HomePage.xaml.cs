using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Windows.Controls;
using WebPage.Connections;
using WpfApp.Components;
using WpfApp.DTO.Products;
using WpfApp.Pages.Dialogs;
using WpfApp.Utilities;

namespace WpfApp.Pages.Home {
    public partial class HomePage : Page {
        public HomePage () {
            InitializeComponent ();
            LoadCategoryCards ();
        }

        public async void LoadCategoryCards () {
            try {
                HttpResponseMessage? response = await HttpClientHelper.GetAsync (ProductEndpoints.GetCategories);

                if (!response.IsSuccessStatusCode) {
                    MessageDialog.Show ("Error", "No se pudieron cargar las categorías", AlertType.ERROR);
                    return;
                }

                CategoryResponse? result = await response.Content.ReadFromJsonAsync<CategoryResponse> ();

                if (result?.Body is null || result.Body.Count == 0) {
                    MessageDialog.Show ("Info", "No hay categorías disponibles", AlertType.WARNING);
                    return;
                }

                // Map categories to cards
                List<CategorieCard2>? categoryCards = result.Body.Select (cat => {
                    CategorieCard2? card = new CategorieCard2 ();
                    card.BtnCategory.Content = cat.Category;
                    return card;
                }).ToList ();

                ItemFeed.ItemsSource = categoryCards;
            } catch (Exception ex) {
                MessageDialog.Show ("Excepción", ex.Message, AlertType.ERROR);
            }
        }

        public class CategoryResponse {
            [JsonPropertyName ("message")]
            public string Message { get; set; } = "";

            [JsonPropertyName ("body")]
            public List<CategoryDTO> Body { get; set; } = new ();
        }

        private void CardSelected (object sender, EventArgs e) {

        }
    }
}
