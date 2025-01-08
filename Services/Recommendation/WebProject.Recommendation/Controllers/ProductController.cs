using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebProject.Recommendation.Services;

namespace WebProject.Recommendation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductSimilarityService _similarityService;

        public ProductController(IProductSimilarityService similarityService)
        {
            _similarityService = similarityService;
        }

        [HttpGet("featured/{productId}")]
        public async Task<IActionResult> GetFeaturedProducts(string productId)
        {
            var featuredProducts = await _similarityService.GetFeaturedProducts(productId);
            return Ok(featuredProducts);
        }
    }
}
