using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAuctioneerService.Controllers {
    [ApiController]
    [Route ("api/Auction")]
    public class ImagesController : Controller {
        private readonly IConsultImagesService _consultImagesService;

        public ImagesController (IConsultImagesService consultImagesService) {
            _consultImagesService = consultImagesService;
        }

        [HttpGet ("ImageBase64")]
        public async Task<IActionResult> GetCategories ([FromQuery] int auctionId) {
            try {
                MessageResponse<byte[]> response = await _consultImagesService.GetImageAuctionId(auctionId);

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
