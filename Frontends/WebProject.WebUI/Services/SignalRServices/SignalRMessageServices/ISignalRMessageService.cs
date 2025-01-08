using WebProject.DtoLayer.SignalRDtos;

namespace WebProject.WebUI.Services.SignalRServices.SignalRMessageServices
{
    public interface ISignalRMessageService
    {
        Task<List<ResultUnreadedInboxMessageDto>> GetUnreadedInboxMessages(string id);
    }
}
