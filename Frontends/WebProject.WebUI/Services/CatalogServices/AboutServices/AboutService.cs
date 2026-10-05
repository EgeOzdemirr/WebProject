using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.AboutDtos;

namespace WebProject.WebUI.Services.CatalogServices.AboutServices
{
    public class AboutService:IAboutService
    {
        private readonly HttpClient _httpClient;

        public AboutService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateAboutAsync(CreateAboutDto createAboutDto)
        {
            await _httpClient.PostAsJsonAsync<CreateAboutDto>("Abouts", createAboutDto);
        }

        public async Task DeleteAboutAsync(string id)
        {
            // SERVİSLERİ DÜZELT
            await _httpClient.DeleteAsync("Abouts?id=" + id);
        }

        public async Task<List<ResultAboutDto>> GetAllAboutAsync()
        {
            var responseMessage = await _httpClient.GetAsync("Abouts");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultAboutDto>>(jsondata);
            return values;
        }

        public async Task<UpdateAboutDto> GetByIdAboutAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("Abouts/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateAboutDto>(jsondata);
            return value;
        }

        public async Task UpdateAboutAsync(UpdateAboutDto updateAboutDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateAboutDto>("Abouts", updateAboutDto);
        }
    }
}
