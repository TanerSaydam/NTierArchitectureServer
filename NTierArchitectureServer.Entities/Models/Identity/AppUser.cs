using Microsoft.AspNetCore.Identity;

namespace NTierArchitectureServer.Entities.Models.Identity
{
    public class AppUser : IdentityUser<string>
    {
        public byte[] ProfileImage { get; set; }
        public string ResetPasswordCode { get; set; }
    }
}
