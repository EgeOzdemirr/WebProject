using Newtonsoft.Json;
using WebProject.DtoLayer.OrderDtos.OrderAddressDtos;

namespace WebProject.WebUI.Services.OrderServices.OrderAddressServices
{
    public class OrderAddressService : IOrderAddressService
    {
        private readonly HttpClient _httpClient;
        public OrderAddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateAddressAsync(CreateOrderAddressDto createOrderAddressDto)
        {
            await _httpClient.PostAsJsonAsync<CreateOrderAddressDto>("Addresses", createOrderAddressDto);
        }

        public async Task DeleteAddressAsync(int id)
        {
            await _httpClient.DeleteAsync("Addresses/RemoveAddress/" + id);
        }
        public async Task<List<UpdateOrderAddressDto>> GetAddressesByUserIdAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("Addresses/GetAddressListByUserId/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<List<UpdateOrderAddressDto>>(jsondata);
            //var value = await responseMessage.Content.ReadFromJsonAsync<GetByIdCategoryDto>();
            return value;
        }
        public async Task<List<ResultAddressesByUserDto>> GetAllAddressAsync()
        {
            var responseMessage = await _httpClient.GetAsync("Addresses");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<List<ResultAddressesByUserDto>>(jsondata);
            //var value = await responseMessage.Content.ReadFromJsonAsync<GetByIdCategoryDto>();
            return value;
        }
        public async Task<UpdateOrderAddressDto> GetByIdAddressAsync(int id)
        {
            var responseMessage = await _httpClient.GetAsync("Addresses/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateOrderAddressDto>(jsondata);
            //var value = await responseMessage.Content.ReadFromJsonAsync<GetByIdCategoryDto>();
            return value;
        }
        public async Task UpdateAddressAsync(UpdateOrderAddressDto updateOrderAddressDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateOrderAddressDto>("Addresses", updateOrderAddressDto);
        }
    }
}
