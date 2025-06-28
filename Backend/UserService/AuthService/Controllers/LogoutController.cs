using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers {
    [ApiController]
    [Authorize]
    [Route ("api/User/[controller]")]
    public class LogoutController : ControllerBase {
        private readonly IHttpContextAccessor _contextAccesor;

        public LogoutController (IHttpContextAccessor contextAccesor) {
            _contextAccesor = contextAccesor;
        }

        [HttpPost]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> PostLogout () {
            try {
                _contextAccesor.HttpContext?.Response.Cookies.Delete ("jwtToken");
                return Ok ();
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
