using WebProject.DtoLayer.CatalogDtos.ProductDtos;

namespace WebProject.WebUI.Services.CatalogServices.ProductRecommendationServices
{
    public class ProductRecommendationService : IProductRecommendationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProductRecommendationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<List<ResultProductDto>> GetRecommendedProductsAsync(string userId)
        {
            try
            {
                var baseUrl = _configuration["ApiSettings:BaseUrl"];
                var response = await _httpClient.GetAsync($"{baseUrl}/api/Features/GetRecommendedProducts?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<ResultProductDto>>();
                }

                // Hata durumunda boş liste dön
                return new List<ResultProductDto>();
            }
            catch (Exception ex)
            {
                // Hata logla
                Console.WriteLine($"Ürün önerileri alınırken hata oluştu: {ex.Message}");
                return new List<ResultProductDto>();
            }
        }
    }
}
