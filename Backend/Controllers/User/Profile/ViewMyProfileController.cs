using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.User.Profile {
    [Authorize]
    [ApiController]
    [Route ("api/User/Profile/[controller]")]
    public class ViewMyProfileController : Controller {
        private readonly IProfileService _profileService;

        public ViewMyProfileController (IProfileService profileService) {
            _profileService = profileService;
        }

        [Authorize]
        [HttpGet ("ViewMyProfile")]
        public async Task<IActionResult> GetViewMyProfileAsync () {
            return Ok ();
        }
    }
}
