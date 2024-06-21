using AutoMapper;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;

namespace FinTrack.Application.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountDto>()
                
                .ForMember(dest => dest.UserId,
                           src => src.MapFrom(x => x.UserId))
                .ForMember(dest => dest.Balance,
                           src => src.MapFrom(x => x.Balance));

        }
    }
}
