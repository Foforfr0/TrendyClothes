using Backend.Auth;
using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.User.Auth {
    [ApiController]
    [Route ("api/User/Auth/[controller]")]
    public class LogoutController : ControllerBase {
        private readonly IHttpContextAccessor _contextAccesor;
        private readonly ManageJWTToken _manageJWTToken;
        private readonly IAuthService _authService;

        public LogoutController (IHttpContextAccessor contextAccesor, ManageJWTToken manageJWTToken, IAuthService authService) {
            _contextAccesor = contextAccesor;
            _manageJWTToken = manageJWTToken;
            _authService = authService;
        }

        [HttpPost]
        public IActionResult PostLogout () {
            try {
                if (_contextAccesor.HttpContext?.Request.Cookies.TryGetValue ("jwtToken", out string? jwtToken) ?? false)
                    _contextAccesor.HttpContext.Response.Cookies.Delete ("jwtToken");
                return NoContent ();
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
