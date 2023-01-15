using NTierArchitectureServer.Business.Services.EmailTemplateServices.Dtos;
using NTierArchitectureServer.DataAccess.Repositories;
using NTierArchitectureServer.DataAccess.Repositories.EmailTemplateRepository;
using NTierArchitectureServer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Business.Services.EmailTemplateServices
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IUnitOfWork _unitOfWork;
        public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository, IUnitOfWork unitOfWork)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(EmailTemplateDto emailTemplateDto)
        {
            await CheckTitle(emailTemplateDto.Title);

            EmailTemplate emailTemplate = new()
            {
                Id = Guid.NewGuid(),
                Title = emailTemplateDto.Title,
                Content = Encoding.UTF8.GetBytes(emailTemplateDto.Content)
            };
            await _emailTemplateRepository.AddAsync(emailTemplate);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            await _emailTemplateRepository.RemoveByIdAsync(id);
            await _unitOfWork.SaveChangesAsync();            
        }

        public IList<EmailTemplateDto> GetAll()
        {
            return _emailTemplateRepository.GetAll(false).Select(s=> new EmailTemplateDto
            {
                Id = s.Id.ToString(),
                Title = s.Title,
                Content = Encoding.UTF8.GetString(s.Content)
            }).ToList();
        }

        public async Task<EmailTemplateDto> GetByIdAsync(string id)
        {
            var emailTemplate = await _emailTemplateRepository.GetByIdAsync(id);

            return new EmailTemplateDto
            {
                Id = emailTemplate.Id.ToString(),
                Title = emailTemplate.Title,
                Content = Encoding.UTF8.GetString(emailTemplate.Content)
            };
        }

        public async Task<EmailTemplateDto> GetByTitleAsync(string title)
        {
            var emailTemplate = await _emailTemplateRepository.FindFirstAsync(p => p.Title == title);

            if (emailTemplate == null) return null;

            return new EmailTemplateDto
            {
                Id = emailTemplate.Id.ToString(),
                Title = emailTemplate.Title,
                Content = Encoding.UTF8.GetString(emailTemplate.Content)
            };
        }

        public async Task UpdateAsync(EmailTemplateDto emailTemplateDto)
        {
            EmailTemplate emailTemplate = await _emailTemplateRepository.GetByIdAsync(emailTemplateDto.Id);

            if (emailTemplate.Title != emailTemplateDto.Title)
                await CheckTitle(emailTemplateDto.Title);

            emailTemplate.Title = emailTemplateDto.Title;
            emailTemplate.Content = Encoding.UTF8.GetBytes(emailTemplateDto.Content);
            _emailTemplateRepository.Update(emailTemplate);
            await _unitOfWork.SaveChangesAsync(); 
        }

        public async Task CheckTitle(string title)
        {
            var checkTitle = await GetByTitleAsync(title);
            if (checkTitle != null) throw new Exception("Bu başlık daha önce kullanılmış!");
        }
    }
}
