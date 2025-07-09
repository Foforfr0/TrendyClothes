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

        [HttpGet("WonAuctions")]
        public async Task<IActionResult> GetWonAuctions([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new
                {
                    error = true,
                    message = "El username es obligatorio.",
                    body = (object?)null
                });
            }

            var buyerIdResult = await _auctionsDAO.GetBuyerIdByUsernameAsync(username);

            if (buyerIdResult == null || buyerIdResult.IsError || buyerIdResult.DataRetrieved <= 0)
            {
                return BadRequest(new
                {
                    error = true,
                    message = buyerIdResult?.Message ?? "No se pudo obtener el ID del usuario.",
                    body = (object?)null
                });
            }

            var wonAuctionsResult = await _auctionsDAO.GetWonAuctionsByBuyerAsync(buyerIdResult.DataRetrieved);

            if (wonAuctionsResult == null || wonAuctionsResult.IsError)
            {
                return StatusCode(500, new
                {
                    error = true,
                    message = wonAuctionsResult?.Message ?? "Error al recuperar subastas ganadas.",
                    body = (object?)null
                });
            }

            return Ok(new
            {
                error = false,
                message = wonAuctionsResult.Message,
                body = wonAuctionsResult.DataRetrieved
            });
        }



    }

}
