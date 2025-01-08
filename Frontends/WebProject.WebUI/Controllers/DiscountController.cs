using Microsoft.AspNetCore.Mvc;
using WebProject.WebUI.Services.BasketServices;
using WebProject.WebUI.Services.DiscountServices;

namespace WebProject.WebUI.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }
        public async Task<IActionResult> Index(string code)
        {
            var values = await _discountService.GetByCodeDiscountCouponAsync(code);
            ViewData["codeRate"] = values.Rate;
            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
