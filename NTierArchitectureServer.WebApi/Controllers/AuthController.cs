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
            var token = await _authService.LoginAsync(loginDto);            
            return Ok(new { Token = token });
        }

        [HttpGet("[action]/{email}")]
        public async Task<IActionResult> SendConfirmMail(string email)
        {
            await _authService.SendConfirmEmail(email);
            return NoContent();
        }

        [HttpGet("[action]/{email}")]
        public async Task<IActionResult> ConfirmEmail(string email)
        {
            await _authService.ConfirmEmail(email);
            return NoContent(); 
        }

        [HttpGet("[action]/{email}")]
        public async Task<IActionResult> SendResetPasswordEmail(string email)
        {
            await _authService.SendResetPasswordEmail(email);
            return NoContent();
        }
        
        [HttpGet("[action]/{email}/{code}")]
        public async Task<IActionResult> CheckResetPasswordCode(string email, string code)
        {
            await _authService.CheckResetPasswordCode(email,code);
            return NoContent();
        }

        [HttpGet("[action]/{email}/{code}/{password}")]
        public async Task<IActionResult> ResetPassword(string email, string code, string password)
        {
            await _authService.ResetPassword(email, code, password);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateNewToken(RefreshTokenDto refreshToken)
        {
            var token = await _authService.CreateNewToken(refreshToken.RefreshToken);
            return Ok(token);
        }
    }
}
