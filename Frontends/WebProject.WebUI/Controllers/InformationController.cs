using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Controllers
{
    public class InformationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
