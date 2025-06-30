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
    public class CreateProductController : Controller
    {
        private readonly ICreateProductService _createProductService;

        public CreateProductController(ICreateProductService createProductService)
        {
            _createProductService = createProductService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Datos del producto inválidos.");
                }

                MessageResponse<bool> response = await _createProductService.PostProductAsync(request);

                if (response.IsError)
                    return HttpResponses.InternalServerError(response.Message);

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
