using Newtonsoft.Json;
using WebProject.DtoLayer.CatalogDtos.OfferDiscountDtos;

namespace WebProject.WebUI.Services.CatalogServices.OfferDiscountServices
{
    public class OfferDiscountService:IOfferDiscountService
    {
        private readonly HttpClient _httpClient;

        public OfferDiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateOfferDiscountAsync(CreateOfferDiscountDto createOfferDiscountDto)
        {
            await _httpClient.PostAsJsonAsync<CreateOfferDiscountDto>("OfferDiscounts", createOfferDiscountDto);
        }

        public async Task DeleteOfferDiscountAsync(string id)
        {
            await _httpClient.DeleteAsync("OfferDiscounts?id=" + id);
        }

        public async Task<List<ResultOfferDiscountDto>> GetAllOfferDiscountAsync()
        {
            var responseMessage = await _httpClient.GetAsync("OfferDiscounts");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultOfferDiscountDto>>(jsondata);
            return values;
        }

        public async Task<UpdateOfferDiscountDto> GetByIdOfferDiscountAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("OfferDiscounts/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateOfferDiscountDto>(jsondata);
            return value;
        }

        public async Task UpdateOfferDiscountAsync(UpdateOfferDiscountDto updateOfferDiscountDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateOfferDiscountDto>("OfferDiscounts", updateOfferDiscountDto);
        }
    }
}
