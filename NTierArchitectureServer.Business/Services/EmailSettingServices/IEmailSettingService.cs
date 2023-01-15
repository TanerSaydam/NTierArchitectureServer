using NTierArchitectureServer.Business.Services.EmailSettingServices.Dtos;
using NTierArchitectureServer.Entities.Models;

namespace NTierArchitectureServer.Business.Services.EmailSettingServices
{
    public interface IEmailSettingService
    {
        Task UpdateAsync(EmailSettingDto emailSettingDto);
        Task<EmailSetting> GetFirstAsync();
    }
}
