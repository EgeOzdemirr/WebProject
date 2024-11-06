using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebProject.DtoLayer.CommentDtos;
using WebProject.WebUI.Services.CommentServices;

namespace WebProject.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailReviewComponentPartial:ViewComponent
    {
        private readonly ICommentService _commentService;
        public _ProductDetailReviewComponentPartial(ICommentService commentService)
        {
            _commentService = commentService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var values = await _commentService.CommentListByProductId(id);
            return View(values);
        }
    }
}
