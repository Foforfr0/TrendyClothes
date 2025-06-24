using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSellerService.Models;
using ProductSellerService.Services.Intefaces;

namespace ProductSellerService.Controllers {
    [ApiController]
    [Authorize]
    [Route ("api/MyProduct")]
    public class EditProductController : Controller {
        private readonly IEditProductService _editProductService;

        public EditProductController (IEditProductService editProductService) {
            _editProductService = editProductService;
        }

        [HttpPut ("Edit")]
        public async Task<IActionResult> EditProduct ([FromBody] EditProductDTO request) {
            try {
                if (!ModelState.IsValid) {
                    return BadRequest ("Datos del producto inválidos.");
                }
                MessageResponse<bool> response = await _editProductService.PutProductAsync (request);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved == false)
                    return NotFound (new {
                        response.Message
                    });
                return Ok (new {
                    response.Message
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.ToString ());
            }
        }
    }
}
