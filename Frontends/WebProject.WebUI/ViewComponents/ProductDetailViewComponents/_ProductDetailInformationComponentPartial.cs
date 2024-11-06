using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.ProductDetailDtos;
using WebProject.WebUI.Services.CatalogServices.ProductDetailServices;

namespace WebProject.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailInformationComponentPartial:ViewComponent
    {
        private readonly IProductDetailService _productDetailService;
        public _ProductDetailInformationComponentPartial(IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var values = await _productDetailService.GetByProductIdProductDetailAsync(id);
            return View(values);
        }
    }
}
