using AuctionParticipantService.DAO;
using AuctionParticipantService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuctionParticipantService.Controllers
{
    [ApiController]
    [Route("api/Auctions/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly AuctionDAO _auctionsDAO;

        public AuctionController(AuctionDAO auctionsDAO)
        {
            _auctionsDAO = auctionsDAO;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveAuctionsWithPhoto()
        {
            var response = await _auctionsDAO.GetActiveAuctionsWithPhotoAsync();

            if (response.DataRetrieved == null || response.DataRetrieved.Count == 0)
            {
                return NotFound(new { error = true, message = "No se encontraron subastas activas con foto.", body = new List<AuctionDTO>() });
            }

            return Ok(new
            {
                error = false,
                message = response.Message,
                body = response.DataRetrieved
            });
        }

        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetAuctionById(int id)
        {
            var result = await _auctionsDAO.GetAuctionByIdAsync(id);

            if (result.DataRetrieved == null)
            {
                return NotFound(new
                {
                    error = true,
                    message = result.Message,
                    body = (object?)null
                });
            }

            return Ok(new
            {
                error = false,
                message = result.Message,
                body = result.DataRetrieved
            });
        }

        [HttpPut("IncreaseBid/{auctionId}")]
        public async Task<IActionResult> IncreaseLastPrice(int auctionId)
        {
            var result = await _auctionsDAO.IncreaseLastPriceAsync(auctionId);

            if (!result.DataRetrieved)
            {
                return BadRequest(new
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

        [HttpPost("RegisterBid")]
        public async Task<IActionResult> RegisterBid([FromBody] BidDTO bid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Datos de puja inválidos.",
                    body = false
                });
            }

            var result = await _auctionsDAO.RegisterBidAsync(bid);

            if (!result.DataRetrieved)
            {
                return BadRequest(new
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

        [HttpPut("UpdateExpiredAuctions")]
        public async Task<IActionResult> UpdateExpiredAuctions()
        {
            var result = await _auctionsDAO.UpdateExpiredAuctionsAsync();

            if (!result.DataRetrieved)
            {
                return Ok(new
                {
                    error = false,
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
