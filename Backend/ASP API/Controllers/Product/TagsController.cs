using Backend.DTO;
using Backend.DTO.Product;
using Backend.Services.Intefaces.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Controllers.Product {
    [ApiController]
    [Route ("api/Product/[controller]")]
    public class TagsController : Controller {
        private readonly IConsultTagsService _consultTagsService;
        private readonly ILogger<TagsController> _logger;

        public TagsController (IConsultTagsService consultTagsService, ILogger<TagsController> logger) {
            _consultTagsService = consultTagsService;
            _logger = logger;
        }

        [HttpGet ("Categories")]
        public async Task<IActionResult> GetCategories () {
            try {
                MessageResponse<List<CategoriesDTO>> response = await _consultTagsService.GetCategories ();

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

        [HttpGet ("Types")]
        public async Task<IActionResult> GetTypes () {
            try {
                MessageResponse<List<TypesDTO>> response = await _consultTagsService.GetTypes ();

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

        [HttpGet ("Statusses")]
        public async Task<IActionResult> GetStatusses () {
            try {
                MessageResponse<List<StatussesDTO>> response = await _consultTagsService.GetStatusses ();

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
