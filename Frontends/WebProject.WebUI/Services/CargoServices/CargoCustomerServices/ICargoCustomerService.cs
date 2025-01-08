using WebProject.DtoLayer.CargoDtos.CargoCustomerDtos;

namespace WebProject.WebUI.Services.CargoServices.CargoCustomerServices
{
    public interface ICargoCustomerService
    {
        Task<GetCargoCustomerAddressByIdDto> GetCargoCustomerAddressByUserIdAsync(string id);
        Task CreateCargoCustomerAddressAsync(CreateCargoCustomerAddressDto createCargoCustomerAddressDto);
        Task UpdateCargoCustomerAddressAsync(UpdateCargoCustomerAddressDto updateCargoCustomerAddressDto);
        Task DeleteCargoCustomerAddressAsync(int id);
        Task<UpdateCargoCustomerAddressDto> GetByIdCargoCustomerAddressAsync(int id);
    }
}
