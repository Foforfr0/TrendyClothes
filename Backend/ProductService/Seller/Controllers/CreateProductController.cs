using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSellerService.Models;
using ProductSellerService.Services.Interfaces;

namespace ProductSellerService.Controllers {
    [ApiController]
    [Authorize]
    [Route ("api/MyProduct")]
    public class CreateProductController : Controller {
        private readonly ICreateProductService _createProductService;

        public CreateProductController (ICreateProductService createProductService) {
            _createProductService = createProductService;
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct ([FromBody] NewProductDTO newProduct) {
            try {
                if (!ModelState.IsValid)
                    return BadRequest (new {
                        message = $"Los datos del nuevo producto son inválidos."
                    });
                if (string.IsNullOrEmpty (User.Identity?.Name ?? ""))
                    return BadRequest (new {
                        message = $"No se pudo recuperar el nombre de usuario."
                    });
                newProduct.UsernameSeller = User.Identity?.Name ?? string.Empty;
                MessageResponse<int> response = await _createProductService.PostProductAsync (newProduct);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved <= 0)
                    return Conflict (new {
                        response.Message
                    });
                return Ok (new {
                    response.Message,
                    productId = response.DataRetrieved
                });
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
