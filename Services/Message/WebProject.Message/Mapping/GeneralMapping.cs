using AutoMapper;
using WebProject.Message.DAL.Entities;
using WebProject.Message.Dtos;

namespace WebProject.Message.Mapping
{
    public class GeneralMapping:Profile
    {
        public GeneralMapping()
        {
            CreateMap<UserMessage,ResultMessageDto>().ReverseMap();
            CreateMap<UserMessage,CreateMessageDto>().ReverseMap();
            CreateMap<UserMessage,UpdateMessageDto>().ReverseMap();
            CreateMap<UserMessage,GetByIdMessageDto>().ReverseMap();
            CreateMap<UserMessage,ResultInboxMessageDto>().ReverseMap();
            CreateMap<UserMessage,ResultSendboxMessageDto>().ReverseMap();
        }
    }
}
