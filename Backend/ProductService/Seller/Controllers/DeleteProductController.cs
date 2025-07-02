using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSellerService.Models;
using ProductSellerService.Services.Interfaces;

namespace ProductSellerService.Controllers {
    [ApiController]
    [Authorize]
    [Route ("api/MyProduct")]
    public class DeleteProductController : Controller {
        private readonly IDeleteProductService _deleteProductService;

        public DeleteProductController (IDeleteProductService deleteProductService) {
            _deleteProductService = deleteProductService;
        }

        [HttpDelete ("Delete")]
        public async Task<IActionResult> DeleteProductAsync ([FromQuery] int id) {
            try {
                if (id <= 0)
                    return BadRequest ("ID del producto es requerido.");
                MessageResponse<bool> response = await _deleteProductService.DeleteUserAsync (id);
                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (!response.DataRetrieved)
                    return Conflict (new {
                        response.Message
                    });
                return NoContent ();
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
