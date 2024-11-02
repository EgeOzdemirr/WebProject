using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
