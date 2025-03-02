﻿using Microsoft.AspNetCore.Mvc;
using WebProject.DtoLayer.OrderDtos.OrderOrderingDtos;
using WebProject.WebUI.Services.Interfaces;
using WebProject.WebUI.Services.OrderServices.OrderOrderingServices;

namespace WebProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Order")]
    public class OrderController : Controller
    {
        private readonly IOrderOrderingService _orderOrderingService;
        private readonly IUserService _userService;

        public OrderController(IOrderOrderingService orderOrderingService, IUserService userService)
        {
            _orderOrderingService = orderOrderingService;
            _userService = userService;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Siparişler";
            ViewBag.v3 = "Sipariş Listesi";
            ViewBag.t = "Sipariş İşlemleri";
            List<ResultAllOrderingListDto> ModelOrders = new List<ResultAllOrderingListDto>();
            var appUsers = await _userService.GetAllUserInfo();
            var orders = await _orderOrderingService.GetAllOrderingAsync();
            foreach (var item in orders)
            {
                var appUser = appUsers.Where(x => x.Id == item.UserId).FirstOrDefault();
                ResultAllOrderingListDto modelOrder = new ResultAllOrderingListDto()
                {
                    OrderDate = item.OrderDate,
                    OrderingId = item.OrderingId,
                    TotalPrice = item.TotalPrice,
                    NameSurname = appUser.Name + " " + appUser.Surname
                };
                ModelOrders.Add(modelOrder);
            }
            return View(ModelOrders);
        }
    }
}
