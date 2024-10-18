using AutoMapper;
using TopUpAPI.DataMapper;
using TopUpAPI.Models;

namespace TelecomProviderAPI.Infrastructure

{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Beneficiary, opt => opt.MapFrom(src => src.Beneficiaries));
            CreateMap<Beneficiary, BeneficiaryDto>();
            CreateMap<TopUpOption, TopUpOptionDto>();
        }
    }
}
