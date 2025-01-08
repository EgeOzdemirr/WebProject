using WebProject.Catalog.Dtos.ProductDtos;
using WebProject.Catalog.Entities;

namespace WebProject.Catalog.Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ResultProductDto>> GetAllProductAsync();
        Task<List<ResultProductWithCategoryDto>> GetProductsByCategoryIdAsync(string CategoryId);
        Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryAsync();
        Task CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(UpdateProductDto updateProductDto);
        Task DeleteProductAsync(string id);
        Task<GetByIdProductDto> GetByIdProductAsync(string id);
        //Task<List<Product>> GetSimilarProducts(string productId);
    }
}
