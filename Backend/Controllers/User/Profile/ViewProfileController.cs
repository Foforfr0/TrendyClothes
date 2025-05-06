using Backend.Services.Intefaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.User.Profile {
    [ApiController]
    [Route ("api/User/Profile/[controller]")]
    public class ViewProfileController : Controller {
        private readonly IProfileService _profileService;

        public ViewProfileController (IProfileService profileService) {
            _profileService = profileService;
        }

        [Authorize]
        [HttpGet ("ViewProfile")]
        public async Task<IActionResult> GetViewMyProfileAsync () {
            return Ok ();
        }
    }
}
