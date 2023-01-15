using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NTierArchitectureServer.Business.Services.AuthServices;
using NTierArchitectureServer.Business.Services.AuthServices.Dtos;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace NTierArchitectureServer.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            ResultDto result = await _authService.RegisterAsync(registerDto);
            if(!result.IsSucces)
                return BadRequest(result);

            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            await _authService.LoginAsync(loginDto);
            //düzeltilecek
            return Ok();
        }

        [HttpGet("[action]/{email}")]
        public async Task<IActionResult> SendConfirmMail(string email)
        {
            await _authService.SendConfirmEmail(email);
            return NoContent();
        }
    }
}
