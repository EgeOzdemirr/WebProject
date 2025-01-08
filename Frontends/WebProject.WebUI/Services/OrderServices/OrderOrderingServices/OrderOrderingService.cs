using Newtonsoft.Json;
using WebProject.DtoLayer.CommentDtos;
using WebProject.DtoLayer.OrderDtos.OrderOrderingDtos;

namespace WebProject.WebUI.Services.OrderServices.OrderOrderingServices
{
    public class OrderOrderingService : IOrderOrderingService
    {
        private readonly HttpClient _httpClient;
        public OrderOrderingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateOrderingAsync(CreateOrderingDto createOrderingDto)
        {
            await _httpClient.PostAsJsonAsync<CreateOrderingDto>("Orderings", createOrderingDto);
        }
        public async Task DeleteOrderingAsync(int id)
        {
            await _httpClient.DeleteAsync("Orderings/RemoveOrdering/" + id);
        }
        public async Task<List<ResultOrderingByUserIdDto>> GetAllOrderingAsync()
        {
            var responseMessage = await _httpClient.GetAsync("Orderings");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<List<ResultOrderingByUserIdDto>>(jsondata);
            //var value = await responseMessage.Content.ReadFromJsonAsync<GetByIdCategoryDto>();
            return value;
        }
        public async Task<UpdateOrderingDto> GetByIdOrderingAsync(int id)
        {
            var responseMessage = await _httpClient.GetAsync("Orderings/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateOrderingDto>(jsondata);
            //var value = await responseMessage.Content.ReadFromJsonAsync<GetByIdCategoryDto>();
            return value;
        }
        public async Task<List<ResultOrderingByUserIdDto>> GetOrderingByUserIdAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("Orderings/GetOrderingByUserId/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<List<ResultOrderingByUserIdDto>>(jsondata);
            //var value = await responseMessage.Content.ReadFromJsonAsync<GetByIdCategoryDto>();
            return value;
        }
        public async Task UpdateOrderingAsync(UpdateOrderingDto updateOrderingDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateOrderingDto>("Orderings", updateOrderingDto);
        }
    }
}
