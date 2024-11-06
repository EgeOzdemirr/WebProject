using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.FeatureDtos;
using WebProject.WebUI.Services.CatalogServices.FeatureServices;

namespace WebProject.WebUI.ViewComponents.DefaultViewComponents
{
    public class _FeatureDefaultComponentPartial:ViewComponent
    {
        private readonly IFeatureService _featureService;

        public _FeatureDefaultComponentPartial(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _featureService.GetAllFeatureAsync();
            return View(values);
        }
    }
}
