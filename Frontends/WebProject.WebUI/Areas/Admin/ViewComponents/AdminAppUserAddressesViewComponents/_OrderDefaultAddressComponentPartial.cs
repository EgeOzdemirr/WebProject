using Microsoft.AspNetCore.Mvc;
using WebProject.DtoLayer.OrderDtos.OrderAddressDtos;
using WebProject.WebUI.Services.OrderServices.OrderAddressServices;

namespace WebProject.WebUI.Areas.Admin.ViewComponents.AdminAppUserAddressesViewComponents
{
    public class _OrderDefaultAddressComponentPartial : ViewComponent
    {
        private readonly IOrderAddressService _orderAddressService;
        public _OrderDefaultAddressComponentPartial(IOrderAddressService orderAddressService)
        {
            _orderAddressService = orderAddressService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var appUserId = ViewData["auId"].ToString();
            var addresses = await _orderAddressService.GetAddressesByUserIdAsync(appUserId);
            var defaultAddress = addresses.Where(x => x.Isdefault == true).FirstOrDefault();

            if (defaultAddress != null)
            {
                return View(defaultAddress);
            }
            else
            {
                UpdateOrderAddressDto? model = new UpdateOrderAddressDto();
                return View(model);
            }
        }
    }
}
