using Microsoft.AspNetCore.Mvc;
using WebProject.DtoLayer.CommentDtos;

namespace WebProject.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ReviewsFormProductDetailComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            CreateCommentDto? m = new CreateCommentDto();
            return View(m);
        }
    }
}
