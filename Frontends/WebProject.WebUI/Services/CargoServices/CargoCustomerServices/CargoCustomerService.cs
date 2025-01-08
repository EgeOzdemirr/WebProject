using Newtonsoft.Json;
using System.Net.Http.Json;
using WebProject.DtoLayer.CargoDtos.CargoCustomerDtos;

namespace WebProject.WebUI.Services.CargoServices.CargoCustomerServices
{
    public class CargoCustomerService : ICargoCustomerService
    {
        private readonly HttpClient _httpClient;

        public CargoCustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateCargoCustomerAddressAsync(CreateCargoCustomerAddressDto createCargoCustomerAddressDto)
        {
            await _httpClient.PostAsJsonAsync<CreateCargoCustomerAddressDto>("CargoCustomers", createCargoCustomerAddressDto);
        }

        public async Task DeleteCargoCustomerAddressAsync(int id)
        {
            await _httpClient.DeleteAsync("CargoCustomers/RemoveCargoCustomer/" + id);
        }

        public async Task<GetCargoCustomerAddressByIdDto> GetCargoCustomerAddressByUserIdAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("CargoCustomers/GetCargoCustomerByUserId/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<GetCargoCustomerAddressByIdDto>(jsondata);

            return values;
        }

        public async Task<UpdateCargoCustomerAddressDto> GetByIdCargoCustomerAddressAsync(int id)
        {
            var responseMessage = await _httpClient.GetAsync("CargoCustomers/GetCargoCustomerById/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateCargoCustomerAddressDto>(jsondata);
            //var value = await responseMessage.Content.ReadFromJsonAsync<GetByIdCategoryDto>();
            return value;
        }

        public async Task UpdateCargoCustomerAddressAsync(UpdateCargoCustomerAddressDto updateCargoCustomerAddressDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateCargoCustomerAddressDto>("CargoCustomers", updateCargoCustomerAddressDto);
        }
    }
}
