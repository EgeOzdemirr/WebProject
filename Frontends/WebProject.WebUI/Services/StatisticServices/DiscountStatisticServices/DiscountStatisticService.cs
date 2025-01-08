
using Newtonsoft.Json;

namespace WebProject.WebUI.Services.StatisticServices.DiscountStatisticServices
{
    public class DiscountStatisticService : IDiscountStatisticService
    {
        private readonly HttpClient _httpClient;
        public DiscountStatisticService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> GetActiveDiscountCouponsCount()
        {
            var responseMessage = await _httpClient.GetAsync("Discounts/GetActiveDiscountCouponsCount");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<int>(jsondata);
            return values;
        }
        public async Task<int> GetDiscountCouponsCount()
        {
            var responseMessage = await _httpClient.GetAsync("Discounts/GetDiscountCouponsCount");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<int>(jsondata);
            return values;
        }
        public async Task<int> GetPassiveDiscountCouponsCount()
        {
            var responseMessage = await _httpClient.GetAsync("Discounts/GetPassiveDiscountCouponsCount");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<int>(jsondata);
            return values;
        }
    }
}
