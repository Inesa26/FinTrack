using AutoMapper;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;

namespace FinTrack.Application.Profiles
{
    public class MonthlySummaryProfile : Profile
    {
        public MonthlySummaryProfile()
        {
            CreateMap<MonthlySummary, MonthlySummaryDto>()
                .ForMember(dest => dest.Year,
                           src => src.MapFrom(x => x.Year))
                .ForMember(dest => dest.Month,
                           src => src.MapFrom(x => x.Month))
                .ForMember(dest => dest.Income,
                           src => src.MapFrom(x => x.Income))
                .ForMember(dest => dest.Expenses,
                           src => src.MapFrom(x => x.Expenses))
                .ForMember(dest => dest.Balance,
                           src => src.MapFrom(x => x.Balance));

        }
    }
}
