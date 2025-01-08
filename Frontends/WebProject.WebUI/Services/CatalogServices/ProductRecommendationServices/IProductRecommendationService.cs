using WebProject.DtoLayer.CatalogDtos.ProductDtos;

namespace WebProject.WebUI.Services.CatalogServices.ProductRecommendationServices
{
    public interface IProductRecommendationService
    {
        Task<List<ResultProductDto>> GetRecommendedProductsAsync(string userId);
    }
}
