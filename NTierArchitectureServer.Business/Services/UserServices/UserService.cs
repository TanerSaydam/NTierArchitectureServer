using GenericFileService.Files;
using Microsoft.AspNetCore.Identity;
using MimeTypes;
using NTierArchitectureServer.Business.Services.UserServices.Dtos;
using NTierArchitectureServer.Core.Utilies;
using NTierArchitectureServer.Entities.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Business.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task UpdateProfileImage(ChangeUserProfileImageDto changeUserProfileImageDto)
        {
            List<string> allowTypes = new List<string> 
            { 
                MimeTypeMap.GetMimeType("jpeg"),
                MimeTypeMap.GetMimeType("jpg") };
            FileControlService.CheckFileType(changeUserProfileImageDto.File, allowTypes);
            FileControlService.CheckFileSize(changeUserProfileImageDto.File, 5);            

            AppUser user = await _userManager.FindByIdAsync(changeUserProfileImageDto.Id);

            if (user == null) throw new Exception("Kullanıcı bulunamadı!");

            user.ProfileImage = FileService.FileConvertByteArrayToDatabase(changeUserProfileImageDto.File);
            user.ProfileImageUrl = FileService.FileSaveToServer(changeUserProfileImageDto.File, "./wwwroot/images/users/");

            //FileSaveToFtpModel fileSaveToFtpModel = new(
            //    "ftp adresi //ftp://ftp.tanersaydam.com/wwwroot/images/users/",
            //    "kullanıcı adı",
            //    "şifre");

            //user.ProfileImageUrl = FileService.FileSaveToFtp(changeUserProfileImageDto.File,fileSaveToFtpModel);

            await _userManager.UpdateAsync(user);
        }
    }
}
