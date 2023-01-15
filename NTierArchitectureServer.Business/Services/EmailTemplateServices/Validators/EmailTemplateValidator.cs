using FluentValidation;
using NTierArchitectureServer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Business.Services.EmailTemplateServices.Validators
{
    public class EmailTemplateValidator : AbstractValidator<EmailTemplate>
    {
        public EmailTemplateValidator()
        {
            RuleFor(p => p.Title).NotEmpty().WithMessage("Başlık yazmalısınız!");
            RuleFor(p => p.Title).NotNull().WithMessage("Başlık yazmalısınız!");
            RuleFor(p => p.Content).NotEmpty().WithMessage("İçerik yazmalısınız!");
            RuleFor(p => p.Content).NotNull().WithMessage("İçerik yazmalısınız!");
        }
    }
}
