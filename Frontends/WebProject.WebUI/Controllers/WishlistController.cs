using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebProject.WebUI.Services.CatalogServices.ProductServices;
using WebProject.WebUI.Services.WishlistServices;

namespace WebProject.WebUI.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;
        private readonly IProductService _productService;

        public WishlistController(IWishlistService wishlistService, IProductService productService)
        {
            _wishlistService = wishlistService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Dr1 = "Anasayfa";
            ViewBag.Dr2 = "/Default/Index/";
            ViewBag.Dr3 = "Hesabım";
            ViewBag.Dr4 = "/Wishlist/Index/";
            ViewBag.Dr5 = "Favorilerim";

            var ids = await _wishlistService.GetIdsAsync();
            var all = await _productService.GetProductsWithCategoryAsync();
            var items = ids
                .Select(id => all.FirstOrDefault(p => p.ProductId == id))
                .Where(p => p != null)
                .ToList();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(string id)
        {
            var added = await _wishlistService.ToggleAsync(id);
            var count = (await _wishlistService.GetIdsAsync()).Count;
            return Json(new
            {
                added,
                count,
                message = added ? "Favorilere eklendi" : "Favorilerden çıkarıldı"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string id)
        {
            await _wishlistService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
