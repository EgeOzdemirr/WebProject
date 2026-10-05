using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.ProductDtos;
using WebProject.WebUI.Services.CatalogServices.CategoryServices;
using WebProject.WebUI.Services.CatalogServices.ProductServices;
using WebProject.WebUI.Services.WishlistServices;

namespace WebProject.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListComponentPartial:ViewComponent
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWishlistService _wishlistService;
        public _ProductListComponentPartial(IProductService productService, ICategoryService categoryService, IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            ViewBag.WishIds = User.Identity?.IsAuthenticated == true
                ? (await _wishlistService.GetIdsAsync()).ToHashSet()
                : new HashSet<string>();
            if (id == null)
            {
                var values = await _productService.GetProductsWithCategoryAsync();
                return View(values);
            }
            else
            {
                var values2 = await _categoryService.GetByIdCategoryAsync(id);
                if (values2 == null)
                {
                    return View(new List<ResultProductWithCategoryDto>());
                }

                var values = await _categoryService.GetProductsByCategoryIdAsync(id);
                ViewBag.ct = values.Count > 0 ? values2.CategoryName + " " + "Kategorisindeki Ürünler" : values2.CategoryName + " " + "Kategorisinde Henüz Ürün Yok";
                return View(values);
            }
        }
    }
}
