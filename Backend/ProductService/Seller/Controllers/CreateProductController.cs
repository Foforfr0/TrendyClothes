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
        private readonly ILogger<CreateProductController> _logger;
        public CreateProductController(ICreateProductService createProductService, ILogger<CreateProductController> logger)
        {
            _createProductService = createProductService;
            _logger = logger;
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid product data: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
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
                _logger.LogWarning("Invalid product data: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return HttpResponses.InternalServerError(ex.ToString());
            }
        }
    }
}
