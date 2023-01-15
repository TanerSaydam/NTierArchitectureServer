using AutoMapper;
using NTierArchitectureServer.Business.Services.EmailSettingServices.Dtos;
using NTierArchitectureServer.Entities.Models;

namespace NTierArchitectureServer.Business.MappingProfiles
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailSettingDto, EmailSetting>().ReverseMap();
        }
    }
}
