using AutoMapper;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;

namespace FinTrack.Application.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Id,
                           src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.Title,
                           src => src.MapFrom(x => x.Title))
                .ForMember(dest => dest.IconId,
                           src => src.MapFrom(x => x.IconId))
                .ForMember(dest => dest.Type,
                           src => src.MapFrom(x => x.Type));

            CreateMap<Category, CategoryIconDto>()
               .ForMember(dest => dest.Id,
                          src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.Title,
                          src => src.MapFrom(x => x.Title))
               .ForMember(dest => dest.Icon,
                           src => src.Ignore());
        }
    }
}
