using Microsoft.AspNetCore.Mvc;
using WebProject.WebUI.Services.BasketServices;

namespace WebProject.WebUI.ViewComponents.ShoppingCartViewComponents
{
    public class _BasketTotalAmountComponentPartial : ViewComponent
    {
        private readonly IBasketService _basketService;

        public _BasketTotalAmountComponentPartial(IBasketService basketService)
        {
            _basketService = basketService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int rate = 0;
            string codeName = null;
            if (ViewData["codeRate"] != null)
            {
                rate = int.Parse(ViewData["codeRate"].ToString());
                codeName = ViewData["codeName"].ToString();
            }
            var totalValue = await _basketService.GetBasket(null);
            totalValue.DiscountRate = rate;
            totalValue.DiscountCode = codeName;
            return View(totalValue);
        }
    }
}
