using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListColorFilterComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
