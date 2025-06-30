using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSellerService.DAO;
using ProductSellerService.Models;
using ProductSellerService.Services.Interfaces;
namespace ProductSellerService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/MyProduct")]
    public class DeleteProductController : Controller
    {
        private readonly IDeleteProductService _deleteProductService;

        public DeleteProductController(IDeleteProductService deleteProductService)
        {
            _deleteProductService = deleteProductService;
        }

        [HttpPatch("Delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                MessageResponse<bool> response = await _deleteProductService.DeleteProductAsync(id);

                if (response.IsError)
                    return HttpResponses.InternalServerError(response.Message);

                if (response.DataRetrieved == false)
                    return NotFound(new
                    {
                        response.Message
                    });

                return Ok(new
                {
                    response.Message
                });
            }
            catch (Exception ex)
            {
                return HttpResponses.InternalServerError(ex.ToString());
            }
        }
    }
}
