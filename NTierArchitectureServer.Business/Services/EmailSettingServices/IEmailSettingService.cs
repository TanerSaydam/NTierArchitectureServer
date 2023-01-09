using NTierArchitectureServer.Entities.Models;

namespace NTierArchitectureServer.Business.Services.EmailSettingServices
{
    public interface IEmailSettingService
    {
        Task UpdateAsync(EmailSetting emailSetting);
        Task<EmailSetting> GetFirstAsync();
    }
}
