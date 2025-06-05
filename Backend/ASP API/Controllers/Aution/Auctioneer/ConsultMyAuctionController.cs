using Backend.DTO;
using Backend.DTO.Auction.Consult;
using Backend.Services.Intefaces.Auction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Aution.Auctioneer {
    [ApiController]
    [Authorize]
    [Route ("api/Auction/Auctioneer/[controller]")]
    public class ConsultMyAuctionController : Controller {
        private readonly IConsultAuctionService _consultAuctionService;

        public ConsultMyAuctionController (IConsultAuctionService consultAuctionService) {
            _consultAuctionService = consultAuctionService;
        }

        [HttpGet ("MyAuctions")]
        public async Task<IActionResult> MyAuctions (string? username) {
            try {
                if (string.IsNullOrEmpty (username))
                    username = User.Identity?.Name;
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("No se logró leer el nombre de usuario.");

                MessageResponse<List<AuctionsDTO>> response = await _consultAuctionService.GetAuctionsByUserAsync (username);

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

        [HttpGet ("ViewMyAuctionDetails")]
        public async Task<IActionResult> ViewMyAuctionDetails (int id) {
            try {
                if (id == 0)
                    return BadRequest ("Identificador de subasta requerido.");

                MessageResponse<AuctionDetailsDTO> response = await _consultAuctionService.GetAuctionAsync (id);

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
