using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Areas.User.Controllers
{
    public class CargoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
