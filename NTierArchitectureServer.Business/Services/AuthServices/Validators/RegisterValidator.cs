using FluentValidation;
using NTierArchitectureServer.Business.Services.AuthServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Business.Services.AuthServices.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(p => p.Email).NotEmpty().WithMessage("Mail adresi boş olamaz!");
            RuleFor(p => p.Email).NotNull().WithMessage("Mail adresi boş olamaz!");
            RuleFor(p => p.Email).EmailAddress().WithMessage("Geçerli bir mail adresi girin!");
            RuleFor(p => p.UserName).NotEmpty().WithMessage("Kullanıcı adı girmelisiniz!");
            RuleFor(p => p.UserName).NotNull().WithMessage("Kullanıcı adı girmelisiniz!");
            RuleFor(p => p.Password).MinimumLength(6).WithMessage("Şifreniz en az 6 karakter olmalıdır!");
            RuleFor(p => p.Password).Matches("[A-Z]").WithMessage("Şifreniz en az 1 adet büyük harf içermelidir");
            RuleFor(p => p.Password).Matches("[a-z]").WithMessage("Şifreniz en az 1 adet küçük harf içermelidir");
            RuleFor(p => p.Password).Matches("[0-9]").WithMessage("Şifreniz en az 1 adet rakam içermelidir");
            RuleFor(p => p.Password).Matches("[^a-zA-Z0-9]").WithMessage("Şifreniz en az 1 adet özel karakter içermelidir");
        }
    }
}
