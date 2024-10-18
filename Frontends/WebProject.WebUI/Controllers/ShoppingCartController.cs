using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
