using Microsoft.AspNetCore.Mvc;
using WebProject.WebUI.Services.BasketServices;
using WebProject.WebUI.Services.Interfaces;

namespace WebProject.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IBasketService _basketService;

        public PaymentController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public IActionResult Index()
        {
            ViewBag.Dr1 = "Anasayfa";
            ViewBag.Dr2 = "/Default/Index/";
            ViewBag.Dr3 = "Ödeme";
            return View();
        }

        [HttpPost]
        public IActionResult CompletePayment()
        {
            // Ödeme işlemi burada yapılacak (şu an için varsayımsal)

            // Ödeme başarılı olduğunda, kullanıcıya Sipariş alındı mesajını göster.
            TempData["PaymentSuccessMessage"] = "Siparişiniz alınmıştır.";

            return RedirectToAction("OrderSuccess");
        }
        public IActionResult OrderSuccess()
        {
            // OrderSuccess sayfasında başarılı ödeme mesajını göster.
            return View();
        }
    }
}