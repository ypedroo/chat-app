using AutoMapper;
using Jobsity.Chat.Domain.Dto;
using Jobsity.Chat.Domain.Entities;

namespace Jobsity.Chat.Domain.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        this.CreateMap<MessageDto, Message>()
            .ForMember(dest => dest.Username,
                opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Text,
                opt => opt.MapFrom(src => src.Message))
            .ForMember(dest => dest.CreationDate,
                opt => opt.MapFrom(src => src.CreationDate));
    }
}