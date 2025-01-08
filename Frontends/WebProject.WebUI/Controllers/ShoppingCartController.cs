using Microsoft.AspNetCore.Mvc;
using WebProject.DtoLayer.BasketDtos;
using WebProject.WebUI.Models;
using WebProject.WebUI.Services.BasketServices;
using WebProject.WebUI.Services.CatalogServices.ProductServices;
using WebProject.WebUI.Services.DiscountServices;

namespace WebProject.WebUI.Controllers
{
    [Route("ShoppingCart")]
    public class ShoppingCartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        private readonly IDiscountService _discountService;

        public ShoppingCartController(IProductService productService, IBasketService basketService, IDiscountService discountService)
        {
            _productService = productService;
            _basketService = basketService;
            _discountService = discountService;
        }
        [Route("Index")]
        [Route("Index/{code}")]
        public async Task<IActionResult> Index(string? code)
        {
            ViewBag.Dr1 = "Anasayfa";
            ViewBag.Dr2 = "/Default/Index/";
            ViewBag.Dr3 = "Ürünler";
            ViewBag.Dr4 = "/ProductList/Index/";
            ViewBag.Dr5 = "Sepetim";
            if (code != null)
            {
                var values = await _discountService.GetByCodeDiscountCouponAsync(code);
                ViewData["codeRate"] = values.Rate;
                ViewData["codeName"] = values.Code;
                ViewBag.codeName = values.Code;
            }
            return View();
        }

        [Route("AddBasketItem/{productId}")]
        [Route("AddBasketItem/")]
        public async Task<IActionResult> AddBasketItem(string productId)
        {
            var values = await _productService.GetByIdProductAsync(productId);
            var item = new BasketItemDto
            {
                ProductId = values.ProductId,
                ProductName = values.ProductName,
                Price = values.ProductPrice,
                ImageUrl = values.ProductImageUrl,
                Quantity = 1
            };
            await _basketService.AddBasketItem(item);
            return RedirectToAction("Index");
        }

        [Route("UpdateQuantity/{productId}/{quantity}")]
        public async Task<IActionResult> UpdateQuantity(string productId, int quantity)
        {

                var result = await _basketService.UpdateQuantity(productId, quantity);
                if (result)
                {
                    var basket = await _basketService.GetBasket(null); // Yeniden sepete bak.
                    return PartialView("_BasketTotalAmountComponentPartial", basket);
                }

                return BadRequest(); // Gerekirse hata döndür.
        }

        [Route("RemoveBasketItem/{productId}")]
        public async Task<IActionResult> RemoveBasketItem(string productId)
        {
            await _basketService.RemoveBasketItem(productId);
            return RedirectToAction("Index");
        }
    }
}
