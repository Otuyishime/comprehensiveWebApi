using System;
using AutoMapper;
using testWebAPI.Models.Entities;
using testWebAPI.Models.Resources;

namespace testWebAPI.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RoomEntity, Room>()
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate / 100.0m));
            // TODO: Handle Href
        }
    }
}
