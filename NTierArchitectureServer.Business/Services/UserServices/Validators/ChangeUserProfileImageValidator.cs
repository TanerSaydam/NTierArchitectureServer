using FluentValidation;
using NTierArchitectureServer.Business.Services.UserServices.Dtos;

namespace NTierArchitectureServer.Business.Services.UserServices.Validators
{
    public class ChangeUserProfileImageValidator : AbstractValidator<ChangeUserProfileImageDto>
    {
        public ChangeUserProfileImageValidator()
        {
            RuleFor(p => p.Id).NotEmpty().WithMessage("Kullanıcı Id'si boş olamaz!");
            RuleFor(p => p.Id).NotNull().WithMessage("Kullanıcı Id'si boş olamaz!");
            RuleFor(p => p.File).NotNull().WithMessage("Resim seçmelisiniz!");
        }
    }
}
