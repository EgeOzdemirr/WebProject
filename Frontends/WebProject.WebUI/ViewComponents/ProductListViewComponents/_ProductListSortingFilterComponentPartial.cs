using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListSortingFilterComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
