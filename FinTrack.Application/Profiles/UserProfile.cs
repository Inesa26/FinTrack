using AutoMapper;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;

namespace FinTrack.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
               .ForMember(dest => dest.Id,
                          src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.FirstName,
                          src => src.MapFrom(x => x.FirstName))
                .ForMember(dest => dest.LastName,
                          src => src.MapFrom(x => x.LastName))
                .ForMember(dest => dest.Email,
                          src => src.MapFrom(x => x.Email));
        }
    }
}
