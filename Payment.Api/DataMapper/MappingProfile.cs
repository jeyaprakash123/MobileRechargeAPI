using AutoMapper;
using BalanceApi.Models;

namespace BalanceApi.DataMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Balance, BalanceDto>();
             
        }
    }
}
