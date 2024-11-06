using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.ProductDtos;
using WebProject.WebUI.Services.CatalogServices.ProductServices;

namespace WebProject.WebUI.ViewComponents.DefaultViewComponents
{
    public class _FeatureProductsDefaultComponentPartial:ViewComponent
    {
        private readonly IProductService _productService;

        public _FeatureProductsDefaultComponentPartial(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _productService.GetAllProductAsync();
            return View(values);
        }
    }
}
