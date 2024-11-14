using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.directory1 = "Web Project";
            ViewBag.directory2 = "Ödeme Ekranı";
            ViewBag.directory3 = "Kartla Ödeme";
            return View();
        }
    }
}
