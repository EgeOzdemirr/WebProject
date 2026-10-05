using Microsoft.AspNetCore.Mvc;
using WebProject.DtoLayer.CatalogDtos.ProductDtos;
using WebProject.WebUI.Services;
using WebProject.WebUI.Services.CatalogServices.ProductRecommendationServices;
using WebProject.WebUI.Services.CatalogServices.ProductServices;
using WebProject.WebUI.Services.WishlistServices;

namespace WebProject.WebUI.ViewComponents.DefaultViewComponents
{
    public class _FeatureProductsDefaultComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IProductRecommendationService _recommendationService;
        private readonly IWishlistService _wishlistService;

        public _FeatureProductsDefaultComponentPartial(
            IProductService productService,
            IProductRecommendationService recommendationService,
            IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
            _productService = productService;
            _recommendationService = recommendationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var userId = User.Identity?.IsAuthenticated == true ? User.Identity.Name : null;

                List<ResultProductDto> products;

                if (!string.IsNullOrEmpty(userId))
                {
                    // Kullanıcı giriş yapmışsa önerilen ürünleri göster
                    products = await _recommendationService.GetRecommendedProductsAsync(userId);

                    // Eğer öneri yoksa veya hata olduysa tüm ürünleri göster
                    if (!products.Any())
                    {
                        products = await _productService.GetAllProductAsync();
                        products = products.OrderBy(x => Guid.NewGuid()).Take(4).ToList();
                    }
                }
                else
                {
                    // Kullanıcı giriş yapmamışsa rastgele 4 ürün göster
                    products = await _productService.GetAllProductAsync();
                    products = products.OrderBy(x => Guid.NewGuid()).Take(4).ToList();
                }

                ViewBag.WishIds = User.Identity?.IsAuthenticated == true
                    ? (await _wishlistService.GetIdsAsync()).ToHashSet()
                    : new HashSet<string>();
                return View(products);
            }
            catch (Exception ex)
            {
                // Hata durumunda boş liste ile devam et
                Console.WriteLine($"Ürünler getirilirken hata oluştu: {ex.Message}");
                return View(new List<ResultProductDto>());
            }
        }
    }
}