using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.ProductDetailDtos;

namespace WebProject.WebUI.Services.CatalogServices.ProductDetailServices
{
    public class ProductDetailService:IProductDetailService
    {
        private readonly HttpClient _httpClient;
        public ProductDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto)
        {
            await _httpClient.PostAsJsonAsync<CreateProductDetailDto>("ProductDetails", createProductDetailDto);
        }
        public async Task DeleteProductDetailAsync(string id)
        {
            await _httpClient.DeleteAsync("ProductDetails?id=" + id);
        }
        public async Task<List<ResultProductDetailDto>> GetAllProductDetailAsync()
        {
            var responseMessage = await _httpClient.GetAsync("ProductDetails");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductDetailDto>>(jsondata);
            return values;
        }
        public async Task<UpdateProductDetailDto> GetByIdProductDetailAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("ProductDetails/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateProductDetailDto>(jsondata);
            return value;
        }
        public async Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateProductDetailDto>("ProductDetails", updateProductDetailDto);
        }
        public async Task<UpdateProductDetailDto> GetByProductIdProductDetailAsync(string id)
        {

            var responseMessage = await _httpClient.GetAsync("ProductDetails/GetProductDetailByProductId/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<UpdateProductDetailDto>(jsondata);
            return values;
        }
    }
}
