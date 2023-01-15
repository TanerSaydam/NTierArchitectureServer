using GenericEmailService;
using Microsoft.AspNetCore.Identity;
using NTierArchitectureServer.Business.Services.AuthServices.Dtos;
using NTierArchitectureServer.Business.Services.EmailSettingServices;
using NTierArchitectureServer.Business.Services.EmailTemplateServices;
using NTierArchitectureServer.Entities.Models.Identity;

namespace NTierArchitectureServer.Business.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSettingService _emailSettingService;
        private readonly IEmailTemplateService _emailTemplateService;
        public AuthService(UserManager<AppUser> userManager, IEmailSettingService emailSettingService, IEmailTemplateService emailTemplateService)
        {
            _userManager = userManager;
            _emailSettingService = emailSettingService;
            _emailTemplateService = emailTemplateService;
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

            await SendConfirmEmail(registerDto.Email);

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

        public async Task SendConfirmEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("Kullanıcı bulunamadı!");

            if (user.EmailConfirmed) throw new Exception("Mail adresi daha önce onaylanmış!");

            var emailSetting = await _emailSettingService.GetFirstAsync();
            var emailTemplate = await _emailTemplateService.GetByTitleAsync("Register");
            SendEmailModel sendEmailModel = new();
            sendEmailModel.Email = emailSetting.Email;
            sendEmailModel.Password = emailSetting.Password;
            sendEmailModel.Html = emailSetting.HTML;
            sendEmailModel.SSL = emailSetting.SSL;
            sendEmailModel.Smtp = emailSetting.SMTP;
            sendEmailModel.Port = emailSetting.Port;
            sendEmailModel.Subject = "Mail Onayı!";
            sendEmailModel.Body = emailTemplate.Content.Replace("$url", "https://localhost7014/api/confirmMail/" + email);
            sendEmailModel.Emails = new List<string> { email };

            await EmailService.SendEmailAsync(sendEmailModel);
        }
    }
}
