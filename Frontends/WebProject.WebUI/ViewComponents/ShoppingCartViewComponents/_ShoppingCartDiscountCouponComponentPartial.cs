using Microsoft.AspNetCore.Mvc;
using WebProject.WebUI.Services.DiscountServices;

namespace WebProject.WebUI.ViewComponents.ShoppingCartViewComponents
{
    public class _ShoppingCartDiscountCouponComponentPartial : ViewComponent
    {
        private readonly IDiscountService _discountService;

        public _ShoppingCartDiscountCouponComponentPartial(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var activeCoupons = await _discountService.GetActiveDiscountCouponsAsync();
            return View(activeCoupons);
        }
    }
}
