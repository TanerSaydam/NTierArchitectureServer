using Microsoft.AspNetCore.Identity;

namespace NTierArchitectureServer.Entities.Models.Identity
{
    public class AppUser : IdentityUser<string>
    {
        public byte[] ProfileImage { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ResetPasswordCode { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set;}
    }
}
