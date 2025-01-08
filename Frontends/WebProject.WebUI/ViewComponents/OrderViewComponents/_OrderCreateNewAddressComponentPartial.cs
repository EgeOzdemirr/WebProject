using Microsoft.AspNetCore.Mvc;
using WebProject.DtoLayer.OrderDtos.OrderAddressDtos;
using WebProject.WebUI.Services.OrderServices.OrderAddressServices;

namespace WebProject.WebUI.ViewComponents.OrderViewComponents
{
    public class _OrderCreateNewAddressComponentPartial : ViewComponent
    {
        private readonly IOrderAddressService _orderAddressService;
        public _OrderCreateNewAddressComponentPartial(IOrderAddressService orderAddressService)
        {
            _orderAddressService = orderAddressService;
        }
        public IViewComponentResult Invoke()
        {
            CreateOrderAddressDto? addressCreateModel = new CreateOrderAddressDto();
            var appUserId = ViewData["uId"].ToString();
            ViewBag.AppUserId = appUserId;
            return View(addressCreateModel);

        }
    }
}
