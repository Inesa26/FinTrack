using AutoMapper;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;

namespace FinTrack.Application.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDto>()
               .ForMember(dest => dest.Id,
                          src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.Amount,
                          src => src.MapFrom(x => x.Amount))
                .ForMember(dest => dest.Date,
                          src => src.MapFrom(x => x.Date))
                .ForMember(dest => dest.Description,
                          src => src.MapFrom(x => x.Description))
                .ForMember(dest => dest.CategoryId,
                          src => src.MapFrom(x => x.CategoryId));
        }
    }
}
