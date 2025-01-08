using Microsoft.AspNetCore.Mvc;
using WebProject.DtoLayer.CargoDtos.CargoCustomerDtos;
using WebProject.DtoLayer.OrderDtos.OrderAddressDtos;
using WebProject.WebUI.Services.CargoServices.CargoCustomerServices;
using WebProject.WebUI.Services.Interfaces;

namespace WebProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/AppUserAdmin")]
    public class AppUserAdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICargoCustomerService _cargoCustomerService;
        public AppUserAdminController(IUserService userService, ICargoCustomerService cargoCustomerService)
        {
            _userService = userService;
            _cargoCustomerService = cargoCustomerService;
        }

        [Route("AppUserList")]
        public async Task<IActionResult> AppUserList()
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Kullanıcılar";
            ViewBag.v3 = "Kullanıcı Listesi";
            ViewBag.t = "Kullanıcı Listesi";
            var appUsers = await _userService.GetAllUserInfo();
            return View(appUsers);
        }
        [Route("UserCargoAddressInfo/{id}")]
        public async Task<IActionResult> UserCargoAddressInfo(string id)
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Kullanıcı Adres Bilgileri";
            ViewBag.v3 = "Kullanıcı Adres İşlemleri";
            ViewBag.t = "Kullanıcı Adres İşlemleri";
            ViewData["auId"] = id;
            var value = await _cargoCustomerService.GetCargoCustomerAddressByUserIdAsync(id);
            return View(value);
        }
        [Route("UpdateUserCargoAddress")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserCargoAddress(UpdateOrderAddressDto updateOrderAddressDto)
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Kullanıcı Adres Bilgileri";
            ViewBag.v3 = "Kullanıcı Adres İşlemleri";
            ViewBag.t = "Kullanıcı Adres İşlemleri";
            var userCargoAddress = await _cargoCustomerService.GetCargoCustomerAddressByUserIdAsync(updateOrderAddressDto.UserId);
            if (userCargoAddress != null)
            {
                UpdateCargoCustomerAddressDto modelCargoAddress = new UpdateCargoCustomerAddressDto()
                {
                    cargoCustomerId = userCargoAddress.cargoCustomerId,
                    userCustomerId = updateOrderAddressDto.UserId,
                    name = updateOrderAddressDto.Name,
                    surname = updateOrderAddressDto.Surname,
                    email = updateOrderAddressDto.Email,
                    phone = updateOrderAddressDto.Phone,
                    city = updateOrderAddressDto.City,
                    district = updateOrderAddressDto.District,
                    address = updateOrderAddressDto.Detail1 + " " + updateOrderAddressDto.Detail2 + " - " + updateOrderAddressDto.ZipCode + " - " + updateOrderAddressDto.Country + " - " + updateOrderAddressDto.Description
                };

                await _cargoCustomerService.UpdateCargoCustomerAddressAsync(modelCargoAddress);
                return RedirectToAction("UserCargoAddressInfo", new { id = updateOrderAddressDto.UserId });
            }
            else
            {
                CreateCargoCustomerAddressDto modelNewCargoAddress = new CreateCargoCustomerAddressDto()
                {
                    userCustomerId = updateOrderAddressDto.UserId,
                    name = updateOrderAddressDto.Name,
                    surname = updateOrderAddressDto.Surname,
                    email = updateOrderAddressDto.Email,
                    phone = updateOrderAddressDto.Phone,
                    city = updateOrderAddressDto.City,
                    district = updateOrderAddressDto.District,
                    address = updateOrderAddressDto.Detail1 + " " + updateOrderAddressDto.Detail2 + " " + updateOrderAddressDto.Country + " " + updateOrderAddressDto.Description
                };
                await _cargoCustomerService.CreateCargoCustomerAddressAsync(modelNewCargoAddress);
                return RedirectToAction("UserCargoAddressInfo", new { id = updateOrderAddressDto.UserId });
            }
        }
    }
}
