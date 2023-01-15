using FluentValidation;
using NTierArchitectureServer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Business.Services.EmailSettingServices.Validators
{
    public class EmailSettingValidator : AbstractValidator<EmailSetting>
    {
        public EmailSettingValidator()
        {
            RuleFor(p => p.Email).NotNull().WithMessage("Mail adresi boş olamaz!");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Mail adresi boş olamaz!");
            RuleFor(p => p.Email).EmailAddress().WithMessage("Geçerli bir mai adresi girin!");
            RuleFor(p => p.SMTP).NotNull().WithMessage("SMTP adresi boş olamaz!");
            RuleFor(p => p.SMTP).NotEmpty().WithMessage("SMTP adresi boş olamaz!");
            RuleFor(p => p.Password).NotNull().WithMessage("Mail şifresi boş olamaz!");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Mail şifresi boş olamaz!");
            RuleFor(p => p.Port).NotEmpty().WithMessage("Port boş olamaz!");
            RuleFor(p => p.Port).NotNull().WithMessage("Port boş olamaz!");
        }
    }
}
