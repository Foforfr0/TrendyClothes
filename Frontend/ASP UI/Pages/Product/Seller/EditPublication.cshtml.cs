using ImageProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Product.Consult;
using WebPage.DTO.Product.MyProducts;

namespace WebPage.Pages.Product {
    public class EditPublicationModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EditPublicationModel> _logger;
        private readonly ServicesBuilder _services;
        private readonly ImageProductService.ImageProductServiceClient _grpcClient;

        [BindProperty (SupportsGet = true)]
        public int id {
            get; set;
        }
        [BindProperty]
        public EditPublicationDTO? product {
            get; set;
        }
        public List<CategoriesDTO> categories;
        public SelectList categorySelectList;
        public List<TypesDTO> types;
        public SelectList typeSelectList;
        public List<StatussesDTO> statusses;
        public SelectList statusSelectList;
        public EditPublicationModel (IHttpClientFactory httpClientFactory, ServicesBuilder services, 
                                    ImageProductService.ImageProductServiceClient grpcClient, ILogger<EditPublicationModel> logger) {
            _httpClientFactory = httpClientFactory;
            _services = services;
            _grpcClient = grpcClient;
            _logger = logger;
        }


        public async Task OnGetAsync () {
            await InitializeProductData ();
            await InitializeCategoriesList ();
            await InitializeTypesList ();
            await InitializeStatussesList ();
        }

        public async Task<IActionResult> OnPostAsync () {
            try {
                if (!ModelState.IsValid) {
                    return Page ();
                }
                HttpClient httpClient = _httpClientFactory.CreateClient ();

                string cookies = HttpContext.Request.Headers["Cookie"].ToString ();
                if (!string.IsNullOrEmpty (cookies))
                    httpClient.DefaultRequestHeaders.Add ("Cookie", cookies);

                HttpResponseMessage response = await httpClient.PutAsJsonAsync (_services.SellerPutProductDetailsUrl, new {
                    id = product?.id ?? this.id,
                    product.name, product.price,
                    product.discount, product.description,
                    product.stockAvailable, product.categoryId,
                    product.typeId, product.statusId
                });

                _logger.LogInformation ($"id: {product?.id ?? this.id}\n" +
                                        $"name: {product.name}\n" +
                                        $"price: {product.price}\n" +
                                        $"discount: {product.discount}\n" +
                                        $"description: {product.description.Length}\n" +
                                        $"stock: {product.stockAvailable}\n" +
                                        $"categoryId: {product.categoryId}\n" +
                                        $"typeId: {product.typeId}\n" +
                                        $"statusId: {product.statusId}\n");

                if (response.StatusCode == HttpStatusCode.NotFound) {
                    ModelState.AddModelError (string.Empty, "Producto no encontrado o ID incorrecto.");
                    return Page ();
                }

                if (response.StatusCode == HttpStatusCode.BadRequest) {
                    ModelState.AddModelError (string.Empty, "Datos inválidos.");
                    return Page ();
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized) {
                    ModelState.AddModelError (string.Empty, "No autorizado.");
                    return Page ();
                }

                if (response.IsSuccessStatusCode) {
                    return RedirectToPage ($"/Product/Seller/ViewDetails", new {
                        this.id
                    });
                }
                return Page ();
            } catch (Exception ex) {
                ModelState.AddModelError (string.Empty, "Error al modificar la publicación.");
                return Page ();
            }
        }

        private async Task InitializeProductData () {
            product = new EditPublicationDTO ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            string cookies = HttpContext.Request.Headers["Cookie"].ToString ();
            if (!string.IsNullOrEmpty (cookies))
                httpClient.DefaultRequestHeaders.Add ("Cookie", cookies);
            ApiResponse<EditPublicationDTO>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<EditPublicationDTO>> ($"{_services.SellerGetProductDetailsUrl}?id={id}");

            if (response?.body != null) {
                product = response.body;
                ImageProductReply? grpcResponse = await _grpcClient.GetImageAsync (new ImageProductRequest { ProductId = id });
                product.imageBase64 = Convert.ToBase64String (grpcResponse.ImageData.ToByteArray ());
                product.mimeImage = grpcResponse.ImageType;
            }
        }

        private async Task InitializeCategoriesList () {
            categories = new List<CategoriesDTO> ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            ApiResponse<List<CategoriesDTO>>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<List<CategoriesDTO>>> ($"{_services.ProductGetCategoriesUrl}");

            if (response?.body != null) {
                categories = response.body;
                categorySelectList = new SelectList (categories, "id", "category", product.categoryId);
            }
        }

        private async Task InitializeTypesList () {
            types = new List<TypesDTO> ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            ApiResponse<List<TypesDTO>>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<List<TypesDTO>>> ($"{_services.ProductGetTypesUrl}");

            if (response?.body != null) {
                types = response.body;
                typeSelectList = new SelectList (types, "id", "type", product.typeId);
            }
        }

        private async Task InitializeStatussesList () {
            statusses = new List<StatussesDTO> ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            ApiResponse<List<StatussesDTO>>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<List<StatussesDTO>>> ($"{_services.ProductGetStatussesUrl}");

            if (response?.body != null) {
                statusses = response.body;
                statusSelectList = new SelectList (statusses, "id", "status", product.statusId);
            }
        }
    }
}
