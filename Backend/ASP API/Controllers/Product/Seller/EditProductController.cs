using Backend.DTO;
using Backend.DTO.Product.Seller;
using Backend.Services.Intefaces.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Product.Seller {
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
