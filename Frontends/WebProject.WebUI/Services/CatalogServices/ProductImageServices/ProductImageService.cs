using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.ProductImageDtos;

namespace WebProject.WebUI.Services.CatalogServices.ProductImageServices
{
    public class ProductImageService:IProductImageService
    {
        private readonly HttpClient _httpClient;

        public ProductImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateProductImageAsync(CreateProductImageDto createProductImageDto)
        {
            await _httpClient.PostAsJsonAsync<CreateProductImageDto>("ProductImages", createProductImageDto);
        }

        public async Task DeleteProductImageAsync(string id)
        {
            await _httpClient.DeleteAsync("ProductImages?id=" + id);
        }

        public async Task<List<ResultProductImageDto>> GetAllProductImageAsync()
        {
            var responseMessage = await _httpClient.GetAsync("ProductImages");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductImageDto>>(jsondata);
            return values;
        }

        public async Task<UpdateProductImageDto> GetByIdProductImageAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("ProductImages/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateProductImageDto>(jsondata);
            return value;
        }

        public async Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateProductImageDto>("ProductImages", updateProductImageDto);
        }

        public async Task<List<UpdateProductImageDto>> GetByProductIdProductImageAsync(string id)
        {

            var responseMessage = await _httpClient.GetAsync("ProductImages/ProductImageListByProductId/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<UpdateProductImageDto>>(jsondata);
            return values;
        }
    }
}
