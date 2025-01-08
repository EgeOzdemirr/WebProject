using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Dr1 = "Anasayfa";
            ViewBag.Dr2 = "/Default/Index/";
            ViewBag.Dr3 = "Ödeme";
            return View();
        }
    }
}