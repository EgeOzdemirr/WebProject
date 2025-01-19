using Microsoft.AspNetCore.Mvc;
using WebProject.DtoLayer.BasketDtos;
using WebProject.WebUI.Models;
using WebProject.WebUI.Services.BasketServices;
using WebProject.WebUI.Services.CatalogServices.ProductServices;
using WebProject.WebUI.Services.DiscountServices;
using WebProject.WebUI.Services.Interfaces;

namespace WebProject.WebUI.Controllers
{
    [Route("ShoppingCart")]
    public class ShoppingCartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        private readonly IDiscountService _discountService;

        public ShoppingCartController(IProductService productService, IBasketService basketService, IDiscountService discountService, IUserService userService)
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
            var basket = await _basketService.GetBasket(null);
            var existingItem = basket.BasketItems.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                // Ürün zaten sepette, miktarı artır
                existingItem.Quantity += 1;

                // Sepeti güncelle
                await _basketService.SaveBasket(basket);
            }
            else
            {
                var item = new BasketItemDto
                {
                    ProductId = values.ProductId,
                    ProductName = values.ProductName,
                    Price = values.ProductPrice,
                    ImageUrl = values.ProductImageUrl,
                    Quantity = 1
                };
                await _basketService.AddBasketItem(item);
            }
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

        [Route("ShoppingCartUpdate/{productId}/{quantity}")]
        public async Task<IActionResult> ShoppingCardUpdate(string productId, int quantity)
        {
            var values = await _productService.GetByIdProductAsync(productId);
            var items = new BasketItemDto
            {
                ProductId = values.ProductId,
                ProductName = values.ProductName,
                Price = values.ProductPrice,
                ImageUrl = values.ProductImageUrl,
                Quantity = quantity
            };
            await _basketService.AddBasketItem(items);
            return RedirectToAction("Index"); ;
        }

        [Route("RemoveBasketItem/{productId}")]
        public async Task<IActionResult> RemoveBasketItem(string productId)
        {
            await _basketService.RemoveBasketItem(productId);
            return RedirectToAction("Index");
        }
    }
}
