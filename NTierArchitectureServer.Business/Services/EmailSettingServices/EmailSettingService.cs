using AutoMapper;
using NTierArchitectureServer.Business.Services.EmailSettingServices.Dtos;
using NTierArchitectureServer.DataAccess.Repositories;
using NTierArchitectureServer.DataAccess.Repositories.EmailSettingRepository;
using NTierArchitectureServer.Entities.Models;

namespace NTierArchitectureServer.Business.Services.EmailSettingServices
{
    public class EmailSettingService : IEmailSettingService
    {
        private readonly IEmailSettingRepository _emailSettingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmailSettingService(IEmailSettingRepository emailSettingRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _emailSettingRepository = emailSettingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmailSetting> GetFirstAsync()
        {
            return await _emailSettingRepository.GetFirstAsync();
        }

        public async Task UpdateAsync(EmailSettingDto emailSettingDto)
        {
            var originalEmailSetting = await _emailSettingRepository.GetFirstAsync(false);

            var emailSetting = _mapper.Map<EmailSetting>(emailSettingDto);
            emailSetting.Id = originalEmailSetting.Id;
            emailSetting.CreatedDate = originalEmailSetting.CreatedDate;
            
            _emailSettingRepository.Update(emailSetting);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
