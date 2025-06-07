using GetImageProduct;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaveImageProduct;
using System.Net;
using System.Text.Json;
using WebPage.Connections;
using WebPage.DTO;
using WebPage.DTO.Product.Consult;
using WebPage.DTO.Product.MyProducts;

namespace WebPage.Pages.Product.Seller {
    public class EditPublicationModel : PageModel {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EditPublicationModel> _logger;
        private readonly ServicesBuilder _services;
        private readonly GetImageService.GetImageServiceClient _grpcClientGetImage;
        private readonly SaveImageService.SaveImageServiceClient _grpcClientSaveImage;

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
                                    GetImageService.GetImageServiceClient grpcClientGetImage, SaveImageService.SaveImageServiceClient grpcClientSaveImage,
                                    ILogger<EditPublicationModel> logger) {
            _httpClientFactory = httpClientFactory;
            _services = services;
            _grpcClientGetImage = grpcClientGetImage;
            _grpcClientSaveImage = grpcClientSaveImage;
            _logger = logger;
        }


        public async Task OnGetAsync () {
            await InitializeProductData ();
            await InitializeCategoriesList ();
            await InitializeTypesList ();
            await InitializeStatussesList ();
        }

        private async Task<IActionResult> InitializeProductData () {
            product = new EditPublicationDTO ();

            HttpClient? httpClient = _httpClientFactory.CreateClient ();
            string cookies = HttpContext.Request.Headers["Cookie"].ToString ();
            if (!string.IsNullOrEmpty (cookies)) {
                if (httpClient.DefaultRequestHeaders.Contains ("Cookie"))
                    httpClient.DefaultRequestHeaders.Remove ("Cookie");

                httpClient.DefaultRequestHeaders.Add ("Cookie", cookies);
            }
            HttpResponseMessage httpResponse;
            try {
                httpResponse = await httpClient.GetAsync ($"{_services.SellerGetProductDetailsUrl}?id={id}");
            } catch (Exception ex) {
                _logger.LogError (ex, "Error al conectar con el servicio externo: {Url}");
                return StatusCode (500, new {
                    message = "Error al conectar con el servicio."
                });
            }

            if (!httpResponse.IsSuccessStatusCode) {
                _logger.LogError ("Error en GET {Url}: {StatusCode}", _services.SellerGetProductDetailsUrl, httpResponse.StatusCode);
                return httpResponse.StatusCode switch {
                    HttpStatusCode.BadRequest => BadRequest (new { message = "Campos enviados inválidos." }),
                    HttpStatusCode.NotFound => NotFound (new { message = "Registro no encontrado." }),
                    HttpStatusCode.InternalServerError => StatusCode (500, new { message = "Error interno del servidor." }),
                    _ => StatusCode ((int)httpResponse.StatusCode, new { message = "Error inesperado." })
                };
            }

            string json = await httpResponse.Content.ReadAsStringAsync ();
            _logger.LogInformation ("Respuesta JSON: {Json}", json);

            ApiResponse<EditPublicationDTO>? response;
            try {
                response = JsonSerializer.Deserialize<ApiResponse<EditPublicationDTO>> (json, new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                });
            } catch (JsonException ex) {
                _logger.LogError (ex, "Error al deserializar respuesta JSON.");
                return StatusCode (500, new {
                    message = "Error al procesar la respuesta del servidor."
                });
            }


            if (response?.body != null) {
                product = response.body;
                try {
                    GetImageReply? grpcResponse = await _grpcClientGetImage.GetImageAsync (new GetImageRequest { ProductId = id });
                    product.imageBase64 = Convert.ToBase64String (grpcResponse.ImageData.ToByteArray ());
                    product.mimeImage = grpcResponse.ImageType;
                } catch (Exception ex) {
                    _logger.LogError (ex, "Error al obtener la imagen vía gRPC.");
                    return StatusCode (500, new {
                        message = "Error al obtener la imagen del producto."
                    });
                }
            } else {
                _logger.LogWarning ("La respuesta no contiene cuerpo.");
                return NotFound (new {
                    message = "Producto no encontrado."
                });
            }
            return Page ();
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
                SaveImageRequest grpcRequest = new SaveImageRequest {
                    ProductId = imageDTO.IdProduct ?? this.id,
                    ImageBase64 = imageDTO.ImageBase64,
                    MimeType = imageDTO.MimeImage
                };

                SaveImageReply? responseGrpc = await _grpcClientSaveImage.SaveImageAsync (grpcRequest);

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
