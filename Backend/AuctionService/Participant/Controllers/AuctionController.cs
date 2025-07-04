using AuctionParticipantService.DAO;
using Microsoft.AspNetCore.Mvc;

namespace AuctionParticipantService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly AuctionDAO _dao;
        private readonly ILogger<AuctionController> _logger;

        public AuctionController(AuctionDAO dao, ILogger<AuctionController> logger)
        {
            _dao = dao;
            _logger = logger;
        }

        // GET: api/auction/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveAuctions()
        {
            try
            {
                var auctions = await _dao.GetActiveAuctionsAsync();
                return Ok(auctions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetActiveAuctions");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/auction/{id}/lastprice
        [HttpPut("{id}/lastprice")]
        public async Task<IActionResult> UpdateLastPrice(int id, [FromBody] decimal newLastPrice)
        {
            try
            {
                var result = await _dao.UpdateLastPriceAsync(id, newLastPrice);
                if (!result)
                    return NotFound($"No se encontró la subasta con ID {id}");

                return Ok("Último precio actualizado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en UpdateLastPrice para subasta ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/auction/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuctionById(int id)
        {
            try
            {
                var auction = await _dao.GetAuctionByIdAsync(id);

                if (auction == null)
                    return NotFound($"No se encontró la subasta con ID {id}");

                return Ok(auction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetAuctionById para subasta ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

    }
}
