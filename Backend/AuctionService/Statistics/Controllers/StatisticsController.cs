using AuctionStatistics.Models;
using AuctionStatistics.Models.Consult;
using AuctionStatistics.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionStatistics.Controllers {
    [ApiController]
    [Authorize]
    [Route ("api/Auction/Statistics")]
    public class StatisticsController : Controller {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController (IStatisticsService statisticsService) {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<IActionResult> StatisticsAuction ([FromQuery] int idAuction) {
            try {
                string username = User.Identity?.Name ?? "";
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("No se logró obtener el nombre de usuario.");

                MessageResponse<StatisticsAuctionDTO> response = await _statisticsService.GetStatisticsAuction (idAuction);

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

        [HttpGet ("NumberAuctions")]
        public async Task<IActionResult> GetNumberAuctionsWithDateRange ([FromQuery] DateTime? dateStart, [FromQuery] DateTime? dateEnd) {
            try {
                string username = User.Identity?.Name ?? "";
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("No se logró obtener el nombre de usuario.");

                MessageResponse<int> response = await _statisticsService.GetNumberAuctions (username, dateStart, dateEnd);

                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved <= 0)
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

        [HttpGet ("NumberAuctionsByStatus")]
        public async Task<IActionResult> GetNumberAuctionsStatus ([FromQuery] DateTime? dateStart, [FromQuery] DateTime? dateEnd) {
            try {
                string username = User.Identity?.Name ?? "";
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("No se logró obtener el nombre de usuario.");

                MessageResponse<List<StatusesAuctionDTO>> response = await _statisticsService.GetNumberAuctionsByStatus (username, dateStart, dateEnd);

                if (response.IsError)
                    return HttpResponses.InternalServerError (response.Message);
                if (response.DataRetrieved == null || response.DataRetrieved.Count <= 0)
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

        [HttpGet ("GeneralReport")]
        public async Task<IActionResult> GetGeneralReport ([FromQuery] DateTime dateStart, [FromQuery] DateTime dateEnd) {
            try {
                string username = User.Identity?.Name ?? "";
                if (string.IsNullOrEmpty (username))
                    return BadRequest ("No se logró obtener el nombre de usuario.");

                MessageResponse<GeneralReportDTO> response = await _statisticsService.GetGeneralReport (username, dateStart, dateEnd);

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
