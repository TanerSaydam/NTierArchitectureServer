using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NTierArchitectureServer.Entities.Models.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Core.Security
{
    public class TokenHandler : ITokenHandler
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<AppUser> _userManager;
        public TokenHandler(IOptions<JwtOptions> jwtOptions, UserManager<AppUser> userManager)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
        }

        public async Task<string> CreateTokenAsync(AppUser user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Name, user.Id)
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256);

            DateTime tokenExpires = DateTime.Now.AddHours(1);

            JwtSecurityToken jwtSecurityToken = new(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: tokenExpires,
                signingCredentials: signingCredentials);

            JwtSecurityTokenHandler handler = new();
            string token = handler.WriteToken(jwtSecurityToken);

            user.RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            user.RefreshTokenExpires = tokenExpires.AddHours(1);
            await _userManager.UpdateAsync(user);
            return token;
        }
    }
}
