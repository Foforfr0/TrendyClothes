using AuctionAuctioneerService.Models;
using AuctionAuctioneerService.Models.Consult;
using AuctionAuctioneerService.Services.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAuctioneerService.Controllers {
    [ApiController]
    [Authorize]
    [Route ("api/Auction/Auctioneer/ConsultMyAuctions")]
    public class ConsultMyAuctionsController : Controller {
        private readonly IConsultAuctionService _consultAuctionService;

        public ConsultMyAuctionsController (IConsultAuctionService consultAuctionService) {
            _consultAuctionService = consultAuctionService;
        }

        [HttpGet ("MyAuctions")]
        public async Task<IActionResult> MyAuctions () {
            try {
                string username = User.Identity?.Name;
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("No se logró obtener el nombre de usuario.");

                MessageResponse<List<MyAuctionsDTO>> response = await _consultAuctionService.GetAuctionsByUserAsync (username);

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
