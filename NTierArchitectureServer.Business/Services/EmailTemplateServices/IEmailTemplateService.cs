using NTierArchitectureServer.Business.Services.EmailTemplateServices.Dtos;

namespace NTierArchitectureServer.Business.Services.EmailTemplateServices
{
    public interface IEmailTemplateService
    {
        Task AddAsync(EmailTemplateDto emailTemplateDto);
        Task UpdateAsync(EmailTemplateDto emailTemplateDto);
        Task DeleteAsync(string id);
        Task<EmailTemplateDto> GetByIdAsync(string id);
        Task<EmailTemplateDto> GetByTitleAsync(string title);
        IList<EmailTemplateDto> GetAll();
    }
}
