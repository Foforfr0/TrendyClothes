using Backend.DTO;
using Backend.DTO.Auction.Consult;
using Backend.Services.Intefaces.Auction;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Aution.Participant {
    [ApiController]
    [Route ("api/Auction/Participant/[controller]")]
    public class ConsultAuctionController : Controller {
        private readonly IConsultAuctionService _consultAuctionsService;

        public ConsultAuctionController (IConsultAuctionService consultAuctionService) {
            _consultAuctionsService = consultAuctionService;
        }

        [HttpGet ("Search")]
        public async Task<IActionResult> Search ([FromQuery] string? query) {
            try {
                MessageResponse<List<AuctionsDTO>> response;
                if (string.IsNullOrEmpty (query))
                    response = await _consultAuctionsService.GetAuctionsAsync (default);
                else
                    response = await _consultAuctionsService.GetAuctionsAsync (query);

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

        [HttpGet ("ViewDetails")]
        public async Task<IActionResult> ViewDetails ([FromQuery] int id) {
            try {
                MessageResponse<AuctionDetailsDTO> response = await _consultAuctionsService.GetAuctionAsync (id);

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
