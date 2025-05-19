using Backend.Auth;
using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.User.Auth {
    [Authorize]
    [ApiController]
    [Route ("api/User/[controller]")]
    public class LogoutController : ControllerBase {
        private readonly IHttpContextAccessor _contextAccesor;
        private readonly ManageJWTToken _manageJWTToken;
        private readonly IAuthService _authService;

        public LogoutController (IHttpContextAccessor contextAccesor, ManageJWTToken manageJWTToken, IAuthService authService) {
            _contextAccesor = contextAccesor;
            _manageJWTToken = manageJWTToken;
            _authService = authService;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLogout () {
            try {
                Response.Cookies.Delete ("jwtToken");
                _contextAccesor.HttpContext.Response.Cookies.Delete ("jwtToken");
                await _contextAccesor.HttpContext.SignOutAsync("Cookies");
                return Ok ();
            } catch (Exception ex) {
                return HttpResponses.InternalServerError (ex.Message);
            }
        }
    }
}
