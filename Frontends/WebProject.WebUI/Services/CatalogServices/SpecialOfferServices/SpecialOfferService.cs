using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.SpecialOfferDtos;

namespace WebProject.WebUI.Services.CatalogServices.SpecialOfferServices
{
    public class SpecialOfferService:ISpecialOfferService
    {
        private readonly HttpClient _httpClient;

        public SpecialOfferService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateSpecialOfferAsync(CreateSpecialOfferDto createSpecialOfferDto)
        {
            await _httpClient.PostAsJsonAsync<CreateSpecialOfferDto>("SpecialOffers", createSpecialOfferDto);
        }

        public async Task DeleteSpecialOfferAsync(string id)
        {
            await _httpClient.DeleteAsync("SpecialOffers?id=" + id);
        }

        public async Task<List<ResultSpecialOfferDto>> GetAllSpecialOfferAsync()
        {
            var responseMessage = await _httpClient.GetAsync("SpecialOffers");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultSpecialOfferDto>>(jsondata);
            return values;
        }

        public async Task<UpdateSpecialOfferDto> GetByIdSpecialOfferAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("SpecialOffers/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateSpecialOfferDto>(jsondata);
            return value;
        }

        public async Task UpdateSpecialOfferAsync(UpdateSpecialOfferDto updateSpecialOfferDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateSpecialOfferDto>("SpecialOffers", updateSpecialOfferDto);
        }
    }
}
