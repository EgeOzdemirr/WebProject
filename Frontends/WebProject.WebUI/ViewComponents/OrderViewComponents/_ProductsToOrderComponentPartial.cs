using Microsoft.AspNetCore.Mvc;
using WebProject.WebUI.Services.BasketServices;

namespace WebProject.WebUI.ViewComponents.OrderViewComponents
{
    public class _ProductsToOrderComponentPartial : ViewComponent
    {
        private readonly IBasketService _basketService;
        public _ProductsToOrderComponentPartial(IBasketService basketService)
        {
            _basketService = basketService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int rate = 0;
            string codeName = null;
            if (ViewData["codeRateP"] != null)
            {
                rate = int.Parse(ViewData["codeRateP"].ToString());
                codeName = ViewData["codeNameP"].ToString();
            }
            var totalValue = await _basketService.GetBasket(null);
            totalValue.DiscountRate = rate;
            totalValue.DiscountCode = codeName;
            return View(totalValue);
        }
    }
}
