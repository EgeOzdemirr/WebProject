using WebProject.DtoLayer.SignalRDtos;

namespace WebProject.WebUI.Services.SignalRServices.SignalRCommentServices
{
    public interface ISignalRCommentService
    {
        Task<List<ResultUnApprovedComments>> GetUnApprovedComments();
    }
}
