using NTierArchitectureServer.Business.Services.EmailSettingServices.Dtos;
using NTierArchitectureServer.DataAccess.Repositories;
using NTierArchitectureServer.DataAccess.Repositories.EmailSettingRepository;
using NTierArchitectureServer.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierArchitectureServer.Business.Services.EmailSettingServices
{
    public class EmailSettingService : IEmailSettingService
    {
        private readonly IEmailSettingRepository _emailSettingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmailSettingService(IEmailSettingRepository emailSettingRepository, IUnitOfWork unitOfWork)
        {
            _emailSettingRepository = emailSettingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<EmailSetting> GetFirstAsync()
        {
            return await _emailSettingRepository.GetFirstAsync();
        }

        public async Task UpdateAsync(EmailSettingDto emailSettingDto)
        {
            var emailSetting = await _emailSettingRepository.GetFirstAsync();
            emailSetting.HTML = emailSettingDto.HTML;
            emailSetting.SSL = emailSetting.SSL;
            emailSetting.Email = emailSettingDto.Email;
            emailSetting.Password = emailSettingDto.Password;
            emailSetting.SMTP = emailSettingDto.SMTP;
            emailSetting.Port = emailSettingDto.Port;
            
            _emailSettingRepository.Update(emailSetting);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
