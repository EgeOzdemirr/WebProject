using Newtonsoft.Json;
using System.Net.Http.Json;
using WebProject.DtoLayer.CatalogDtos.BrandDtos;

namespace WebProject.WebUI.Services.CatalogServices.BrandServices
{
    public class BrandService : IBrandService
    {
        private readonly HttpClient _httpClient;

        public BrandService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateBrandAsync(CreateBrandDto createBrandDto)
        {
            var response = await _httpClient.PostAsJsonAsync("Brands", createBrandDto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API'den hata yanıtı alındı: {response.StatusCode} - {error}");
            }
        }

        public async Task DeleteBrandAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"Brands?id={id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Marka silinirken hata oluştu: {error}");
            }
        }

        public async Task<List<ResultBrandDto>> GetAllBrandAsync()
        {
            var response = await _httpClient.GetAsync("Brands");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Markalar getirilirken hata oluştu: {response.StatusCode}");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultBrandDto>>(jsonData);
            return values;
        }

        public async Task<UpdateBrandDto> GetByIdBrandAsync(string id)
        {
            var response = await _httpClient.GetAsync($"Brands/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Marka bilgisi getirilirken hata oluştu: {response.StatusCode}");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateBrandDto>(jsonData);
            return value;
        }

        public async Task UpdateBrandAsync(UpdateBrandDto updateBrandDto)
        {
            var response = await _httpClient.PutAsJsonAsync("Brands", updateBrandDto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Marka güncellenirken hata oluştu: {error}");
            }
        }
    }
}
