
using Newtonsoft.Json;

namespace WebProject.WebUI.Services.StatisticServices.CatalogStatisticServices
{
    public class CatalogStatisticService : ICatalogStatisticService
    {
        private readonly HttpClient _httpClient;
        public CatalogStatisticService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<long> GetBrandCount()
        {
            var responseMessage = await _httpClient.GetAsync("Statistics/GetBrandCount");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<long>(jsondata);
            return value;
        }
        public async Task<long> GetCategoryCount()
        {
            var responseMessage = await _httpClient.GetAsync("Statistics/GetCategoryCount");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<long>(jsondata);
            return value;
        }
        public async Task<long> GetProductCount()
        {
            var responseMessage = await _httpClient.GetAsync("Statistics/GetProductCount");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<long>(jsondata);
            return value;
        }
        public async Task<decimal> GetProductAvgPrice()
        {
            var responseMessage = await _httpClient.GetAsync("Statistics/GetProductAvgPrice");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<decimal>(jsondata);
            return value;
        }
        public async Task<string> GetMaxProductPrice()
        {
            var responseMessage = await _httpClient.GetAsync("Statistics/GetMaxProductPrice");
            var value = await responseMessage.Content.ReadAsStringAsync();
            //var jsondata = await responseMessage.Content.ReadAsStringAsync();
            //var value = JsonConvert.DeserializeObject<string>(jsondata);
            return value;
        }

        public async Task<string> GetMinProductPrice()
        {
            var responseMessage = await _httpClient.GetAsync("Statistics/GetMinProductPrice");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            //var value = JsonConvert.DeserializeObject<string>(jsondata);
            return jsondata;
        }
    }
}
