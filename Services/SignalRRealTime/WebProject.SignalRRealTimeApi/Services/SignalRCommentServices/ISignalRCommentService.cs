namespace WebProject.SignalRRealTimeApi.Services.SignalRCommentServices
{
    public interface ISignalRCommentService
    {
        Task<int> GetCommentsCount();
    }
}
