using GenericFileService.Files;
using Microsoft.AspNetCore.Identity;
using NTierArchitectureServer.Business.Services.AuthServices.Dtos;
using NTierArchitectureServer.Business.Services.UserServices.Dtos;
using NTierArchitectureServer.Entities.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Business.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.EmailorUserName);
            if (user == null)
                user = await _userManager.FindByNameAsync(loginDto.EmailorUserName);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            //token işlemleri
        }

        public async Task<ResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null) 
                throw new Exception("Bu mail adresi daha önce alınmış!");

            user = await _userManager.FindByNameAsync(registerDto.UserName);
            if (user != null)
                throw new Exception("Bu kullanıcı adı daha önce alınmış!");

            AppUser appUser = new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };
            IdentityResult result= await _userManager.CreateAsync(appUser,registerDto.Password);

            ResultDto resultDto = new ResultDto();

            if (!result.Succeeded)
            {
                resultDto.IsSucces = false;
                foreach (var error in result.Errors)
                {
                    resultDto.Messages.Add(error.Description);
                }                
            }           
            return resultDto;
        }

        
    }
}
