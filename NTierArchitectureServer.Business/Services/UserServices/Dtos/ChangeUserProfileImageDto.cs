using Microsoft.AspNetCore.Http;

namespace NTierArchitectureServer.Business.Services.UserServices.Dtos
{
    public class ChangeUserProfileImageDto
    {
        public string Id { get; set; }
        public IFormFile File { get; set; }
    }
}
