using WebProject.Catalog.Dtos.ProductDtos;

namespace WebProject.Catalog.Services.ProductRecommendationServices
{
    public interface IProductRecommendationService
    {
        Task<List<ResultProductDto>> GetRecommendedProductsAsync(string userId, int count = 4);
    }
}
