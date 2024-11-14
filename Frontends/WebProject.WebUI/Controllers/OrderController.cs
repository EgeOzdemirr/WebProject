using Microsoft.AspNetCore.Mvc;
using WebProject.DtoLayer.OrderDtos.OrderAddressDtos;
using WebProject.WebUI.Services.Interfaces;
using WebProject.WebUI.Services.OrderServices.OrderAddressServices;

namespace WebProject.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderAddressService _orderAddressService;
        private readonly IUserService _userService;

        public OrderController(IOrderAddressService orderAddressService, IUserService userService)
        {
            _orderAddressService = orderAddressService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.directory1 = "Web Project";
            ViewBag.directory2 = "Siparişler";
            ViewBag.directory3 = "Sipariş İşlemleri";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateOrderAddressDto createOrderAddressDto)
        {
            var values = await _userService.GetUserInfo();
            createOrderAddressDto.UserId = values.Id;
            await _orderAddressService.CreateOrderAddressAsync(createOrderAddressDto);
            return RedirectToAction("Index", "Payment");
        }
    }
}
