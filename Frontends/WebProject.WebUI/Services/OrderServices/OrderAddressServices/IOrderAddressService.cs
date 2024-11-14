using WebProject.DtoLayer.OrderDtos.OrderAddressDtos;

namespace WebProject.WebUI.Services.OrderServices.OrderAddressServices
{
    public interface IOrderAddressService
    {
        //Task<List<ResultAboutDto>> GetAllAboutAsync();
        Task CreateOrderAddressAsync(CreateOrderAddressDto createOrderAdressDto);
        //Task UpdateAboutAsync(UpdateAboutDto updateAboutDto);
        //Task DeleteAboutAsync(string id);
        //Task<UpdateAboutDto> GetByIdAboutAsync(string id);
    }
}
