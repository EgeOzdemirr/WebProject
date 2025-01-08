using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebProject.Catalog.Dtos.FeatureDtos;
using WebProject.Catalog.Services.FeatureServices;
using WebProject.Catalog.Services.ProductRecommendationServices;

namespace WebProject.Catalog.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IFeatureService _featureService;
        private readonly IProductRecommendationService _productRecommendationService;

        public FeaturesController(IFeatureService featureService, IProductRecommendationService productRecommendationService)
        {
            _featureService = featureService;
            _productRecommendationService = productRecommendationService;
        }

        [HttpGet]
        public async Task<IActionResult> FeatureList()
        {
            var values = await _featureService.GetAllFeatureAsync();
            return Ok(values);
        }

        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommendedProducts()
        {
            var userId = User.Identity.Name; // veya Claims'den alın
            var recommendations = await _productRecommendationService.GetRecommendedProductsAsync(userId);
            return Ok(recommendations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeatureById(string id)
        {
            var values = await _featureService.GetByIdFeatureAsync(id);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeature(CreateFeatureDto featureDto)
        {
            await _featureService.CreateFeatureAsync(featureDto);
            return Ok("Öne çıkan alan başarıyla eklendi");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFeature(string id)
        {
            await _featureService.DeleteFeatureAsync(id);
            return Ok("Öne çıkan alan başarıyla silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeature(UpdateFeatureDto updateFeatureDto)
        {
            await _featureService.UpdateFeatureAsync(updateFeatureDto);
            return Ok("Öne çıkan alan başarıyla güncellendi");
        }
    }
}

