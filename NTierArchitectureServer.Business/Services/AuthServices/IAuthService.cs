using NTierArchitectureServer.Business.Services.AuthServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Business.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ResultDto> RegisterAsync(RegisterDto registerDto);
        Task LoginAsync(LoginDto loginDto);
        Task SendConfirmEmail(string email);
        
    }
}
