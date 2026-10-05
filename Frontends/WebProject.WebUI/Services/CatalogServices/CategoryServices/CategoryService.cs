using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.CategoryDtos;
using WebProject.DtoLayer.CatalogDtos.ProductDtos;

namespace WebProject.WebUI.Services.CatalogServices.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            await _httpClient.PostAsJsonAsync<CreateCategoryDto>("categories", createCategoryDto);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _httpClient.DeleteAsync("Categories?id=" + id);
        }

        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
            var responseMessage = await _httpClient.GetAsync("Categories");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsondata);
            return values;
        }

        public async Task<UpdateCategoryDto> GetByIdCategoryAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("Categories/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateCategoryDto>(jsondata);
            return value;
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateCategoryDto>("Categories", updateCategoryDto);
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsByCategoryIdAsync(string CategoryId)
        {

            var responseMessage = await _httpClient.GetAsync("Products/GetProductsByCategoryIdAsync/" + CategoryId);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsondata);
            return values;
        }
    }
}
