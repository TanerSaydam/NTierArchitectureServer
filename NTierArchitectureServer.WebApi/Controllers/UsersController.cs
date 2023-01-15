using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NTierArchitectureServer.Business.Services.UserServices;
using NTierArchitectureServer.Business.Services.UserServices.Dtos;

namespace NTierArchitectureServer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateProfileImage([FromForm] ChangeUserProfileImageDto changeUserProfileImageDto)
        {
            await _userService.UpdateProfileImage(changeUserProfileImageDto);
            return NoContent();
        }
    }
}
