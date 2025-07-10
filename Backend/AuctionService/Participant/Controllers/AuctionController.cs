using AuctionParticipantService.DAO;
using AuctionParticipantService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionParticipantService.Controllers {
    [ApiController]
    [Route ("api/Auctions/Auction")]
    public class AuctionController : ControllerBase {
        private readonly AuctionDAO _auctionsDAO;

        public AuctionController (AuctionDAO auctionsDAO) {
            _auctionsDAO = auctionsDAO;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveAuctionsWithPhoto () {
            var response = await _auctionsDAO.GetActiveAuctionsWithPhotoAsync ();

            if (response.DataRetrieved == null || response.DataRetrieved.Count == 0) {
                return NotFound (new {
                    error = true, message = "No se encontraron subastas activas con foto.", body = new List<AuctionDTO> ()
                });
            }

            return Ok (new {
                error = false,
                message = response.Message,
                body = response.DataRetrieved
            });
        }

        [HttpGet ("ById/{id}")]
        public async Task<IActionResult> GetAuctionById (int id) {
            var result = await _auctionsDAO.GetAuctionByIdAsync (id);

            if (result.DataRetrieved == null) {
                return NotFound (new {
                    error = true,
                    message = result.Message,
                    body = (object?)null
                });
            }

            return Ok (new {
                error = false,
                message = result.Message,
                body = result.DataRetrieved
            });
        }

        [HttpPut ("IncreaseBid/{auctionId}")]
        public async Task<IActionResult> IncreaseLastPrice (int auctionId) {
            var result = await _auctionsDAO.IncreaseLastPriceAsync (auctionId);

            if (!result.DataRetrieved) {
                return BadRequest (new {
                    error = true,
                    message = result.Message,
                    body = false
                });
            }

            return Ok (new {
                error = false,
                message = result.Message,
                body = true
            });
        }

        [HttpPost("RegisterBid")]
        public async Task<IActionResult> RegisterBid([FromBody] BidTemporalyDTO bid)
        {
            if (string.IsNullOrEmpty(bid.username))
            {
                return BadRequest(new
                {
                    error = true,
                    message = "El username es obligatorio.",
                    body = false
                });
            }

            var userIdResult = await _auctionsDAO.GetBuyerIdByUsernameAsync(bid.username);

            if (userIdResult == null)
            {
                return BadRequest(new
                {
                    error = true,
                    message = userIdResult?.Message ?? "Error al obtener el ID del usuario.",
                    body = false
                });
            }


            var finalBid = new BidDTO
            {
                AuctionId = bid.AuctionId,
                BuyerId = userIdResult.DataRetrieved
            };

            var registerResult = await _auctionsDAO.RegisterBidAsync(finalBid);

            if (!registerResult.DataRetrieved)
            {
                return StatusCode(500, new
                {
                    error = true,
                    message = registerResult.Message,
                    body = false
                });
            }

            return Ok(new
            {
                error = false,
                message = registerResult.Message,
                body = true
            });
        }



        [HttpPut ("UpdateExpiredAuctions")]
        public async Task<IActionResult> UpdateExpiredAuctions () {
            var result = await _auctionsDAO.UpdateExpiredAuctionsAsync ();

            if (!result.DataRetrieved) {
                return Ok (new {
                    error = false,
                    message = result.Message,
                    body = false
                });
            }

            return Ok (new {
                error = false,
                message = result.Message,
                body = true
            });
        }

        [HttpGet("WonWithPhoto")]
        public async Task<IActionResult> GetWonAuctionsWithPhoto([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new
                {
                    error = true,
                    message = "El username es obligatorio.",
                    body = new List<AuctionDTO>()
                });
            }

            var userIdResult = await _auctionsDAO.GetBuyerIdByUsernameAsync(username);

            if (userIdResult == null || userIdResult.DataRetrieved == 0)
            {
                return BadRequest(new
                {
                    error = true,
                    message = userIdResult?.Message ?? "Error al obtener el ID del usuario.",
                    body = new List<AuctionDTO>()
                });
            }

            var response = await _auctionsDAO.GetWonAuctionsByBuyerAsync(userIdResult.DataRetrieved);

            if (response.DataRetrieved == null || response.DataRetrieved.Count == 0)
            {
                return NotFound(new
                {
                    error = true,
                    message = "No se encontraron subastas ganadas por este usuario.",
                    body = new List<AuctionDTO>()
                });
            }

            return Ok(new
            {
                error = false,
                message = response.Message,
                body = response.DataRetrieved
            });
        }

        [HttpPut("MarkAsPaid")]
        public async Task<IActionResult> MarkAuctionAsPaid([FromQuery] int auctionId)
        {
            if (auctionId <= 0)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "El ID de la subasta no es válido.",
                    body = false
                });
            }

            var result = await _auctionsDAO.UpdateAuctionStatusToPaidAsync(auctionId);

            if (!result.DataRetrieved)
            {
                return StatusCode(500, new
                {
                    error = true,
                    message = result.Message,
                    body = false
                });
            }

            return Ok(new
            {
                error = false,
                message = result.Message,
                body = true
            });
        }

    }

}
