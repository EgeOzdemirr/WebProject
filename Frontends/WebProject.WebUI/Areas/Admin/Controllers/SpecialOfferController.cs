﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebProject.DtoLayer.CatalogDtos.SpecialOfferDtos;
using WebProject.WebUI.Services.CatalogServices.SpecialOfferServices;

namespace WebProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/SpecialOffer")]
    public class SpecialOfferController : Controller
    {
        private readonly ISpecialOfferService _specialOfferService;

        public SpecialOfferController(ISpecialOfferService specialOfferService)
        {
            _specialOfferService = specialOfferService;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Özel Teklifler";
            ViewBag.v3 = "Özel Teklif Listesi";
            ViewBag.t = "Özel Teklif İşlemleri";

            var values = await _specialOfferService.GetAllSpecialOfferAsync();

            return View(values);
        }
        [HttpGet]
        [Route("CreateSpecialOffer")]
        public IActionResult CreateSpecialOffer()
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Özel Teklifler";
            ViewBag.v3 = "Özel Teklif Ekle";
            ViewBag.t = "Özel Teklif İşlemleri";
            return View();
        }
        [HttpPost]
        [Route("CreateSpecialOffer")]
        public async Task<IActionResult> CreateSpecialOffer(CreateSpecialOfferDto createSpecialOfferDto)
        {
            await _specialOfferService.CreateSpecialOfferAsync(createSpecialOfferDto);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });

        }

        [Route("DeleteSpecialOffer/{id}")]
        public async Task<IActionResult> DeleteSpecialOffer(string id)
        {
            await _specialOfferService.DeleteSpecialOfferAsync(id);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });

        }
        [Route("UpdateSpecialOffer/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateSpecialOffer(string id)
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Özel Teklifler";
            ViewBag.v3 = "Özel Teklif Güncelleme";
            ViewBag.t = "Özel Teklif İşlemleri";
            var values = await _specialOfferService.GetByIdSpecialOfferAsync(id);
            return View(values);

        }
        [Route("UpdateSpecialOffer/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateSpecialOffer(UpdateSpecialOfferDto updateSpecialOfferDto)
        {
            await _specialOfferService.UpdateSpecialOfferAsync(updateSpecialOfferDto);
            return RedirectToAction("Index", "SpecialOffer", new { area = "Admin" });

        }
    }
}
