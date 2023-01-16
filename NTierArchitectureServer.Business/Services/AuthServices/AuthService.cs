using GenericEmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NTierArchitectureServer.Business.Services.AuthServices.Dtos;
using NTierArchitectureServer.Business.Services.EmailSettingServices;
using NTierArchitectureServer.Business.Services.EmailTemplateServices;
using NTierArchitectureServer.Core.Security;
using NTierArchitectureServer.Entities.Models.Identity;

namespace NTierArchitectureServer.Business.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSettingService _emailSettingService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ITokenHandler _tokenHandler;
        public AuthService(UserManager<AppUser> userManager, IEmailSettingService emailSettingService, IEmailTemplateService emailTemplateService, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _emailSettingService = emailSettingService;
            _emailTemplateService = emailTemplateService;
            _tokenHandler = tokenHandler;
        }

        public async Task ConfirmEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("Kullanıcı bulunamadı!");

            if (user.EmailConfirmed) throw new Exception("Mail adresi zaten onaylanmış!");

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.EmailorUserName);
            if (user == null)
                user = await _userManager.FindByNameAsync(loginDto.EmailorUserName);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı!");

            var checkPassword = await _userManager.CheckPasswordAsync(user,loginDto.Password);

            if (!checkPassword) throw new Exception("Şifre bilgisi hatalı!");

            string token = await _tokenHandler.CreateTokenAsync(user);
            return token;
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

        public async Task SendResetPasswordEmail(string email)
        {
            var user = await CheckUserWithEmail(email);

            Random r = new();
            int randNum = r.Next(1000000);
            string resetPasswordCode = randNum.ToString("D6");

            user.ResetPasswordCode = resetPasswordCode;
            await _userManager.UpdateAsync(user);

            SendEmailModel sendEmailModel = await GetSendEmailModel("Reset Password","Şifre Yenileme", new List<string> { email });
            sendEmailModel.Body = sendEmailModel.Body.Replace("$code", resetPasswordCode);

            await EmailService.SendEmailAsync(sendEmailModel);
        }

        public async Task SendConfirmEmail(string email)
        {
            var user = await CheckUserWithEmail(email);

            if (user.EmailConfirmed) throw new Exception("Mail adresi daha önce onaylanmış!");

           SendEmailModel sendEmailModel = await GetSendEmailModel("Register", "Mail Onayı", new List<string> { email });
            sendEmailModel.Body = sendEmailModel.Body.Replace("$url", "https://localhost7014/api/confirmMail/" + email);

            await EmailService.SendEmailAsync(sendEmailModel);
        }

        public async Task<SendEmailModel> GetSendEmailModel(string title, string subject, List<string> emails)
        {
            var emailSetting = await _emailSettingService.GetFirstAsync();
            var emailTemplate = await _emailTemplateService.GetByTitleAsync(title);
            SendEmailModel sendEmailModel = new();
            sendEmailModel.Email = emailSetting.Email;
            sendEmailModel.Password = emailSetting.Password;
            sendEmailModel.Html = emailSetting.HTML;
            sendEmailModel.SSL = emailSetting.SSL;
            sendEmailModel.Smtp = emailSetting.SMTP;
            sendEmailModel.Port = emailSetting.Port;
            sendEmailModel.Subject = subject;
            sendEmailModel.Body = emailTemplate.Content;
            sendEmailModel.Emails = emails;

            return sendEmailModel;
        }

        public async Task<AppUser> CheckUserWithEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("Kullanıcı bulunamadı!");

            return user;
        }

        public async Task CheckResetPasswordCode(string email, string code)
        {
            var user = await CheckUserWithEmail(email);
            if (user.ResetPasswordCode != code) throw new Exception("Kodunuz geçerli değil!");            
        }

        public async Task ResetPassword(string email, string code, string password)
        {
            var user = await CheckUserWithEmail(email);
            if (user.ResetPasswordCode != code) throw new Exception("Kodunuz geçerli değil!");
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);
            user.ResetPasswordCode = null;

            await _userManager.UpdateAsync(user);
        }

        public async Task<string> CreateNewToken(string refreshToken)
        {
            var user = await _userManager.Users.Where(p => p.RefreshToken == refreshToken).FirstOrDefaultAsync();
            if (user == null) throw new Exception("Kullanıcı bulunamadı!");

            if (user.RefreshTokenExpires < DateTime.Now) throw new Exception("Refresh token süreniz bitmiş!");

            var token = await _tokenHandler.CreateTokenAsync(user);
            return token;
        }
    }
}
