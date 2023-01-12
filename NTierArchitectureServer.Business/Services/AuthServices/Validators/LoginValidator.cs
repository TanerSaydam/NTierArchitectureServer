using FluentValidation;
using NTierArchitectureServer.Business.Services.AuthServices.Dtos;

namespace NTierArchitectureServer.Business.Services.AuthServices.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(p => p.EmailorUserName).NotEmpty().WithMessage("Mail ya kullanıcı adı yazmalısınız!");
            RuleFor(p => p.EmailorUserName).NotNull().WithMessage("Mail ya kullanıcı adı yazmalısınız!");            
            RuleFor(p => p.Password).MinimumLength(6).WithMessage("Şifreniz en az 6 karakter olmalıdır!");
            RuleFor(p => p.Password).Matches("[A-Z]").WithMessage("Şifreniz en az 1 adet büyük harf içermelidir");
            RuleFor(p => p.Password).Matches("[a-z]").WithMessage("Şifreniz en az 1 adet küçük harf içermelidir");
            RuleFor(p => p.Password).Matches("[0-9]").WithMessage("Şifreniz en az 1 adet rakam içermelidir");
            RuleFor(p => p.Password).Matches("[^a-zA-Z0-9]").WithMessage("Şifreniz en az 1 adet özel karakter içermelidir");
        }
    }
}
