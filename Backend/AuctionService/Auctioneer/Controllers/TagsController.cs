using AuctionAuctioneerService.Entities;
using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Consult;
using AuctionAuctioneerService.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAuctioneerService.Controllers {
    [ApiController]
    [Route ("api/Auction")]
    public class TagsController : Controller {
        private readonly IConsultTagsService _consultTagsService;

        public TagsController (IConsultTagsService consultTagsService) {
            _consultTagsService = consultTagsService;
        }

        [HttpGet ("Statuses")]
        public async Task<IActionResult> GetCategories ([FromQuery] int auctionId) {
            try {
                MessageResponse<List<StatusAuctionDTO>> response = await _consultTagsService.GetStatuses();

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
