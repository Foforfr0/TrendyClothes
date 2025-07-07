using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaveImageNewProduct;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Product.Consult;
using WebPage.DTO.Product.MyProducts;
using Microsoft.Extensions.Options;

namespace WebPage.Pages.Product.Seller {
    public class RegistrationProductModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EditPublicationModel> _logger;
        private readonly ServicesConfig _services;
        private readonly SaveNewImageService.SaveNewImageServiceClient _grpcSaveNewImage;

        [BindProperty]
        public NewProductDTO? newProduct {
            get; set;
        }

        public List<CategoriesDTO>? categories;
        public SelectList? categorySelectList;
        public List<TypesDTO>? types;
        public SelectList? typeSelectList;
        public List<StatussesDTO>? statusses;
        public SelectList? statusSelectList;

        public RegistrationProductModel (IHttpClientFactory httpClientFactory, IOptions<ServicesConfig> services,
                                    SaveNewImageService.SaveNewImageServiceClient grpcClientSaveNewImage,
                                    ILogger<EditPublicationModel> logger) {
            _httpClientFactory = httpClientFactory;
            _services = services.Value;
            _grpcSaveNewImage = grpcClientSaveNewImage;
            _logger = logger;
        }

        public async Task OnGetAsync () {
            await InitializeCategoriesList ();
            await InitializeTypesList ();
            await InitializeStatussesList ();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostSendImageAsync ([FromBody] ImageProductDTO imageDTO) {
            _logger.LogCritical ("Enter OnPostImageAsync");

            string? requestToken = HttpContext.Request.Headers["RequestVerificationToken"].ToString ();
            string? formToken = HttpContext.Request.Cookies[".AspNetCore.Antiforgery"]; // nombre aproximado

            IAntiforgery? antiforgery = HttpContext.RequestServices.GetRequiredService<IAntiforgery> ();
            await antiforgery.ValidateRequestAsync (HttpContext);

            if (imageDTO == null || imageDTO.IdProduct <= 0 || string.IsNullOrEmpty (imageDTO.ImageBase64) || string.IsNullOrEmpty (imageDTO.MimeImage)) {
                _logger.LogWarning ("Invalid fields: IdProduct={IdProduct}, ImageBase64Length={ImageLength}, MimeImage={MimeImage}",
                          imageDTO.IdProduct,
                          imageDTO.ImageBase64?.Length ?? 0,
                          imageDTO.MimeImage);
                return BadRequest (new {
                    success = false, message = "Campos inválidos."
                });
            }
            _logger.LogInformation ($"idProduct: {imageDTO.IdProduct}\n" +
                                  $"imageBase64Length: {imageDTO.ImageBase64.Length}\n" +
                                  $"mimeImage: {imageDTO.MimeImage}");

            try {
                SaveNewImageRequest grpcRequest = new SaveNewImageRequest {
                    ProductId = imageDTO.IdProduct??1,
                    ImageBase64 = imageDTO.ImageBase64,
                    MimeType = imageDTO.MimeImage
                };

                SaveNewImageReply? responseGrpc = await _grpcSaveNewImage.SaveNewImageAsync (grpcRequest);

                if (!responseGrpc.Success) {
                    _logger.LogWarning ("gRPC call failed: {Message}", responseGrpc.Message);
                    return BadRequest (new {
                        success = false,
                        message = responseGrpc.Message
                    });
                }
                _logger.LogInformation ("Image saved successfully for product {ProductId}", imageDTO.IdProduct);
                return StatusCode (200, new {
                    success = true, message = responseGrpc.Message
                });
            } catch (Exception ex) {
                _logger.LogError (ex, "Error uploading image for product {ProductId}", imageDTO.IdProduct);
                return StatusCode (500, new {
                    success = false, message = "Error interno al guardar la imagen"
                });
            }
        }

        private async Task InitializeCategoriesList () {
            categories = new List<CategoriesDTO> ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            string requestURL = $"http://apigateway{_services.REST.Product.Product.GetCategories}";
            ApiResponse<List<CategoriesDTO>>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<List<CategoriesDTO>>> (requestURL);

            if (response?.body != null) {
                categories = response.body;
                categorySelectList = new SelectList (categories, "id", "category");
            }
        }

        private async Task InitializeTypesList () {
            types = new List<TypesDTO> ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            string requestURL = $"http://apigateway{_services.REST.Product.Product.GetTypes}";
            ApiResponse<List<TypesDTO>>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<List<TypesDTO>>> (requestURL);

            if (response?.body != null) {
                types = response.body;
                typeSelectList = new SelectList (types, "id", "type");
            }
        }

        private async Task InitializeStatussesList () {
            statusses = new List<StatussesDTO> ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            string requestURL = $"http://apigateway{_services.REST.Product.Product.GetStatusses}";
            ApiResponse<List<StatussesDTO>>? response =
                await httpClient.GetFromJsonAsync<ApiResponse<List<StatussesDTO>>> (requestURL);

            if (response?.body != null) {
                statusses = response.body;
                statusSelectList = new SelectList (statusses, "id", "status");
            }
        }
    }
}
