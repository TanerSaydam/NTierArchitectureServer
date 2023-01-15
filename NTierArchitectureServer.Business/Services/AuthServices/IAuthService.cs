using NTierArchitectureServer.Business.Services.AuthServices.Dtos;

namespace NTierArchitectureServer.Business.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ResultDto> RegisterAsync(RegisterDto registerDto);
        Task LoginAsync(LoginDto loginDto);
        Task SendConfirmEmail(string email);
        Task ConfirmEmail(string email);
        Task SendResetPasswordEmail(string email);
        Task CheckResetPasswordCode(string email, string code);
        Task ResetPassword(string email, string code, string password);
    }
}
