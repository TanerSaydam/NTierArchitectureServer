using NTierArchitectureServer.Business.Services.EmailTemplateServices.Dtos;
using NTierArchitectureServer.Entities.Models;

namespace NTierArchitectureServer.Business.Services.EmailTemplateServices
{
    public interface IEmailTemplateService
    {
        Task AddAsync(EmailTemplateDto emailTemplateDto);
        Task UpdateAsync(EmailTemplateDto emailTemplateDto, string id);
        Task DeleteAsync(string id);
        Task<EmailTemplate> GetByIdAsync(string id);
        Task<EmailTemplate> GetByTitleAsync(string title);
        IQueryable<EmailTemplate> GetAll();
    }
}
