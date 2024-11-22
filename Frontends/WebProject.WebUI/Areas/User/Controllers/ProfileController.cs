using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Areas.User.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
