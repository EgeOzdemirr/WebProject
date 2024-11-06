using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.FeatureSliderDtos;
using WebProject.WebUI.Services.CatalogServices.FeatureSliderServices;

namespace WebProject.WebUI.ViewComponents.DefaultViewComponents
{
    public class _CarouselDefaultComponentPartial:ViewComponent
    {
        private readonly IFeatureSliderService _featureSliderService;

        public _CarouselDefaultComponentPartial(IFeatureSliderService featureSliderService)
        {
            _featureSliderService = featureSliderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _featureSliderService.GetAllFeatureSliderAsync();
            return View(values);
        }
    }
}
