using Backend.DTO;
using Backend.DTO.Product.Consult;
using Backend.Services.Intefaces.Product;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Product {
    [ApiController]
    [Route ("api/Product/[controller]")]
    public class ConsultProductsController : Controller {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConsultService _consultProductsService;

        public ConsultProductsController (IHttpContextAccessor contextAccessor, IConsultService consultProdcutsService) {
            _contextAccessor = contextAccessor;
            _consultProductsService = consultProdcutsService;
        }

        [HttpGet ("Search")]
        public async Task<IActionResult> Search ([FromQuery] string? query) {
            try {
                MessageResponse<List<SearchProducts>> response;
                if (string.IsNullOrEmpty(query))
                    response = await _consultProductsService.GetProductsAsync (null);
                else
                    response = await _consultProductsService.GetProductsAsync (query);

                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if(response.dataRetrieved == null)
                    return NotFound(new {
                        response.message
                    });
                return Ok (new {
                    response.message,
                    body = response.dataRetrieved
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.ToString ());
            }
        }

        [HttpGet ("ViewDetails")]
        public async Task<IActionResult> ViewDetails ([FromQuery] int id) {
            try {
                MessageResponse<ViewDetailsDTO> response = await _consultProductsService.ViewDetailsAsync (id);

                if (response.isError)
                    return HttpResponses.InternalServerError (response.message);
                if (response.dataRetrieved == null)
                    return NotFound (new {
                        response.message
                    });
                return Ok (new {
                    response.message,
                    body = response.dataRetrieved
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.ToString ());
            }
        }
    }
}
