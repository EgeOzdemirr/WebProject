using Microsoft.AspNetCore.Mvc;
using WebProject.WebUI.Services.SignalRServices.SignalRCommentServices;

namespace WebProject.WebUI.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutHeaderCommentComponentPartial : ViewComponent
    {
        private readonly ISignalRCommentService _signalRCommentService;
        public _AdminLayoutHeaderCommentComponentPartial(ISignalRCommentService signalRCommentService)
        {
            _signalRCommentService = signalRCommentService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var comments = await _signalRCommentService.GetUnApprovedComments();
            return View(comments);
        }
    }
}
