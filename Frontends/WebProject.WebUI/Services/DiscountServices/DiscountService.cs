using Newtonsoft.Json;
using WebProject.DtoLayer.DiscountDtos;

namespace WebProject.WebUI.Services.DiscountServices
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateDiscountCouponAsync(CreateDiscountCouponDto createCouponDto)
        {
            await _httpClient.PostAsJsonAsync<CreateDiscountCouponDto>("Discount", createCouponDto);
        }

        public async Task DeleteDiscountCouponAsync(int id)
        {
            await _httpClient.DeleteAsync("Discounts/DeleteDiscountCoupon/" + id);
        }

        public async Task<List<ResultDiscountCouponDto>> GetAllDiscountCouponsAsync()
        {
            var responseMessage = await _httpClient.GetAsync("Discount");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<List<ResultDiscountCouponDto>>(jsondata);
            return value;
        }

        public async Task<List<ResultDiscountCouponDto>> GetActiveDiscountCouponsAsync()
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("Discounts/GetActiveDiscountCoupons");
                if (!responseMessage.IsSuccessStatusCode)
                {
                    return new List<ResultDiscountCouponDto>();
                }
                var jsondata = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ResultDiscountCouponDto>>(jsondata) ?? new List<ResultDiscountCouponDto>();
            }
            catch
            {
                return new List<ResultDiscountCouponDto>();
            }
        }

        public async Task<GetByIdDiscountCouponDto> GetByCodeDiscountCouponAsync(string code)
        {
            var responseMessage = await _httpClient.GetAsync("Discounts/GetDiscountCouponByCode/" + code);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<GetByIdDiscountCouponDto>(jsondata);
            return value;
        }

        public async Task<GetByIdDiscountCouponDto> GetByIdDiscountCouponAsync(int id)
        {
            var responseMessage = await _httpClient.GetAsync("Discounts/GetDiscountCouponById/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<GetByIdDiscountCouponDto>(jsondata);
            return value;
        }

        public async Task UpdateDiscountCouponAsync(UpdateDiscountCouponDto updateCouponDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateDiscountCouponDto>("Discount", updateCouponDto);
        }
    }
}
