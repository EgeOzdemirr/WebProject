using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
