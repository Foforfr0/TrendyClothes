using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSellerService.Models;
using ProductSellerService.Services.Intefaces;

namespace ProductSellerService.Controllers {
    [ApiController]
    [Authorize]
    [Route ("api/MyProducts")]
    public class ConsultMyProductsController : Controller {
        private readonly IConsultProductService _consultProductsService;

        public ConsultMyProductsController (IConsultProductService consultProdcutsService) {
            _consultProductsService = consultProdcutsService;
        }

        [HttpGet ("Search")]
        public async Task<IActionResult> MyProducts ([FromQuery] string username) {
            try {
                MessageResponse<List<MyProductsDTO>> response = await _consultProductsService.GetMyProductsAsync (username);

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
                MessageResponse<MyProductDetailsDTO> response = await _consultProductsService.GetMyProductDetailsAsync (id);

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
