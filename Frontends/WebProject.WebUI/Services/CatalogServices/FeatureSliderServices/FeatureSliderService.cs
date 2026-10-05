using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.FeatureSliderDtos;

namespace WebProject.WebUI.Services.CatalogServices.FeatureSliderServices
{
    public class FeatureSliderService:IFeatureSliderService
    {
        private readonly HttpClient _httpClient;

        public FeatureSliderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateFeatureSliderAsync(CreateFeatureSliderDto createFeatureSliderDto)
        {
            await _httpClient.PostAsJsonAsync<CreateFeatureSliderDto>("FeatureSliders", createFeatureSliderDto);
        }

        public async Task DeleteFeatureSliderAsync(string id)
        {
            await _httpClient.DeleteAsync("FeatureSliders?id=" + id);
        }

        public async Task FeatureSliderChangeToFalse(string id)
        {
            await _httpClient.GetAsync("FeatureSliders/ChangeFeatureSilderStatusToFalse/" + id);
        }

        public async Task FeatureSliderChangeToTrue(string id)
        {
            await _httpClient.GetAsync("FeatureSliders/ChangeFeatureSilderStatusToTrue/" + id);
        }

        public async Task<List<ResultFeatureSliderDto>> GetAllFeatureSliderAsync()
        {
            var responseMessage = await _httpClient.GetAsync("FeatureSliders");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultFeatureSliderDto>>(jsondata);
            return values;
        }

        public async Task<UpdateFeatureSliderDto> GetByIdFeatureSliderAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("FeatureSliders/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateFeatureSliderDto>(jsondata);
            return value;
        }

        public async Task UpdateFeatureSliderAsync(UpdateFeatureSliderDto updateFeatureSliderDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateFeatureSliderDto>("FeatureSliders", updateFeatureSliderDto);
        }
    }
}
