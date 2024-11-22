using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Areas.User.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
