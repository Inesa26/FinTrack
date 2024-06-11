﻿using AutoMapper;
using FinTrack.Application.Responses;
using FinTrack.Domain.Model;

namespace FinTrack.Application.Profiles
{/*
    public class IconProfile : Profile
    {
        public IconProfile()
        {
            CreateMap<Icon, IconDto>()
               .ForMember(dest => dest.Id,
                          src => src.MapFrom(x => x.Id))
               .ForMember(dest => dest.Data,
                         src => src.MapFrom(x => x.Data));
        }
    }
}*/
    public class IconProfile : Profile
    {
        public IconProfile()
        {
            CreateMap<Icon, IconDto>()
                .ForMember(dest => dest.Base64Data, opt => opt.MapFrom(src => Convert.ToBase64String(src.Data)))
                .ForMember(dest => dest.TransactionType,
                           src => src.MapFrom(x => x.TransactionType));
        }
    }}
