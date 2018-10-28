using System;
using AutoMapper;
using testWebAPI.Models;
using testWebAPI.Models.Entities;
using testWebAPI.Models.Resources;

namespace testWebAPI.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RoomEntity, Room>()
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate / 100.0m))
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => 
                Link.To(nameof(Controllers.RoomsController.GetRoomByIdAsync), new { roomId = src.Id })));// TODO: Handle Href
        }
    }
}
