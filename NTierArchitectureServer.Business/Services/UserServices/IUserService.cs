using NTierArchitectureServer.Business.Services.UserServices.Dtos;

namespace NTierArchitectureServer.Business.Services.UserServices
{
    public interface IUserService
    {
        Task UpdateProfileImage(ChangeUserProfileImageDto changeUserProfileImageDto);
    }
}
