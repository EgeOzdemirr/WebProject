using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Areas.AppUser.Controllers
{
    [Area("User")]
    public class AppUserLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
