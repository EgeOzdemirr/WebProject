using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using WebProject.WebUI.Services.CatalogServices.ProductServices;

namespace WebProject.WebUI.Controllers
{
    /// <summary>Ürün karşılaştırma: seçilen ürün id'leri (en fazla 4) bir çerezde tutulur, giriş gerekmez.</summary>
    public class CompareController : Controller
    {
        public const string CookieName = "wp_compare";
        public const int Max = 4;
        private static readonly Regex IdPattern = new("^[0-9a-fA-F]{24}$", RegexOptions.Compiled);

        private readonly IProductService _productService;

        public CompareController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Dr1 = "Anasayfa";
            ViewBag.Dr2 = "/Default/Index/";
            ViewBag.Dr3 = "Ürünler";
            ViewBag.Dr4 = "/ProductList/Index/";
            ViewBag.Dr5 = "Ürün Karşılaştırma";

            var ids = ReadIds();
            var all = await _productService.GetProductsWithCategoryAsync();
            var items = ids
                .Select(id => all.FirstOrDefault(p => p.ProductId == id))
                .Where(p => p != null)
                .ToList();
            return View(items);
        }

        [HttpPost]
        public IActionResult Toggle(string id)
        {
            var ids = ReadIds();
            if (!IdPattern.IsMatch(id ?? string.Empty))
            {
                return BadRequest();
            }

            bool added;
            string message;
            if (ids.Remove(id))
            {
                added = false;
                message = "Karşılaştırmadan çıkarıldı";
            }
            else if (ids.Count >= Max)
            {
                return Json(new { added = false, count = ids.Count, full = true, message = $"En fazla {Max} ürün karşılaştırabilirsiniz" });
            }
            else
            {
                ids.Add(id);
                added = true;
                message = "Karşılaştırmaya eklendi";
            }

            WriteIds(ids);
            return Json(new { added, count = ids.Count, full = false, message });
        }

        [HttpPost]
        public IActionResult Remove(string id)
        {
            var ids = ReadIds();
            ids.Remove(id);
            WriteIds(ids);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Clear()
        {
            WriteIds(new List<string>());
            return RedirectToAction(nameof(Index));
        }

        private List<string> ReadIds()
        {
            var raw = Request.Cookies[CookieName] ?? string.Empty;
            return raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Where(x => IdPattern.IsMatch(x))
                .Distinct()
                .Take(Max)
                .ToList();
        }

        private void WriteIds(List<string> ids)
        {
            Response.Cookies.Append(CookieName, string.Join(',', ids), new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = Request.IsHttps
            });
        }
    }
}
