using Microsoft.AspNetCore.Mvc;

namespace AuctionAuctioneerService.Controllers {
    public static class HttpResponses {
        public static IActionResult InternalServerError (string messageError) {
            return new ObjectResult (
                new { error = true, message = messageError }) 
                { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}
