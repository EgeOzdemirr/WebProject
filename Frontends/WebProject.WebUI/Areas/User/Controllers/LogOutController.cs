using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Areas.User.Controllers
{
    public class LogOutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
