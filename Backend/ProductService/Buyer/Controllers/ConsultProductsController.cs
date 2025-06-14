using Microsoft.AspNetCore.Mvc;
using ProductBuyerService.Models;
using ProductBuyerService.Services.Intefaces;

namespace ProductBuyerService.Controllers {
    [ApiController]
    [Route ("api/Product")]
    public class ConsultProductsController : Controller {
        private readonly IConsultProductService _consultProductsService;

        public ConsultProductsController (IConsultProductService consultProdcutsService) {
            _consultProductsService = consultProdcutsService;
        }

        [HttpGet ("Search")]
        public async Task<IActionResult> Search ([FromQuery] string? query) {
            try {
                MessageResponse<List<ProductsDTO>> response;
                if (string.IsNullOrEmpty (query))
                    response = await _consultProductsService.GetProductsAsync (default);
                else
                    response = await _consultProductsService.GetProductsAsync (query);

                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved == null)
                    return NotFound (new {
                        response.Message
                    });
                return Ok (new {
                    response.Message,
                    body = response.DataRetrieved
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.ToString ());
            }
        }

        [HttpGet ("Details")]
        public async Task<IActionResult> ViewDetails ([FromQuery] int id) {
            try {
                MessageResponse<ProductDetailsDTO> response = await _consultProductsService.GetDetailsAsync (id);

                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved == null)
                    return NotFound (new {
                        response.Message
                    });
                return Ok (new {
                    response.Message,
                    body = response.DataRetrieved
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.ToString ());
            }
        }
    }
}
