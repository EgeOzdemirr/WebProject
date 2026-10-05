using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.FeatureDtos;

namespace WebProject.WebUI.Services.CatalogServices.FeatureServices
{
    public class FeatureService:IFeatureService
    {
        private readonly HttpClient _httpClient;

        public FeatureService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateFeatureAsync(CreateFeatureDto createFeatureDto)
        {
            await _httpClient.PostAsJsonAsync<CreateFeatureDto>("Features", createFeatureDto);
        }

        public async Task DeleteFeatureAsync(string id)
        {
            await _httpClient.DeleteAsync("Features?id=" + id);
        }

        public async Task<List<ResultFeatureDto>> GetAllFeatureAsync()
        {
            var responseMessage = await _httpClient.GetAsync("Features");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultFeatureDto>>(jsondata);
            return values;
        }

        public async Task<UpdateFeatureDto> GetByIdFeatureAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("Features/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateFeatureDto>(jsondata);
            return value;
        }

        public async Task UpdateFeatureAsync(UpdateFeatureDto updateFeatureDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateFeatureDto>("Features", updateFeatureDto);
        }
    }
}
