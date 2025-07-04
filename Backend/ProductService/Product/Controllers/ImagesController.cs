using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Services.Interfaces;

namespace ProductService.Controllers {
    [ApiController]
    [Route ("api/Product")]
    public class ImagesController : Controller {
        private readonly IConsultImagesService _consultImagesService;
        private readonly ILogger<TagsController> _logger;

        public ImagesController (IConsultImagesService consultImagesService, ILogger<TagsController> logger) {
            _consultImagesService = consultImagesService;
            _logger = logger;
        }

        [HttpGet ("Image")]
        public async Task<IActionResult> GetCategories ([FromQuery] int productId) {
            try {
                MessageResponse<byte[]> response = await _consultImagesService.GetImageProductId (productId);

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
