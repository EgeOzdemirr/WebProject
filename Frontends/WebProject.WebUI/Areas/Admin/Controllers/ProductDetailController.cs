﻿ using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebProject.DtoLayer.CatalogDtos.ProductDetailDtos;
using WebProject.WebUI.Services.CatalogServices.ProductDetailServices;

namespace WebProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/ProductDetail")]
    public class ProductDetailController : Controller
    {
        private readonly IProductDetailService _productDetailService;

        public ProductDetailController(IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }

        [Route("UpdateProductDetail/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateProductDetail(string id)
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Açıklama ve Bilgi Güncelleme Sayfası";
            ViewBag.t = "Ürün İşlemleri";
            ViewBag.pErrorId = id;

            var values = await _productDetailService.GetByProductIdProductDetailAsync(id);

            ViewBag.pUId = id;
            return View(values);
        }
        [Route("UpdateProductDetail")]
        [HttpPost]
        public async Task<IActionResult> UpdateProductDetail(UpdateProductDetailDto updateProductDetailDto, string pid)
        {
            if (ModelState.IsValid)
            {
                await _productDetailService.UpdateProductDetailAsync(updateProductDetailDto);
                return RedirectToAction("ProductListWithCategory", "Product", new { area = "Admin" });
            }
            TempData["detailUpdateError"] = "Detay Mevcut Değil, Lütfen Ürün Yeni Detay Ekleme Sayfasından Detay Kaydediniz (Bu Sayfa)";
            return RedirectToAction("CreateProductDetail", "ProductDetail", new { area = "Admin", id = pid });
        }
        [HttpGet]
        [Route("CreateProductDetail/{id}")]
        public IActionResult CreateProductDetail(string id)
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Detayı Ekleme Sayfası";
            ViewBag.t = "Ürün İşlemleri";
            ViewBag.pId = id;
            return View();
        }
        [HttpPost]
        [Route("CreateProductDetail")]
        public async Task<IActionResult> CreateProductDetail(CreateProductDetailDto createProductDto)
        {
            var values2 = await _productDetailService.GetByProductIdProductDetailAsync(createProductDto.ProductId);

            if (values2 != null)
            {
                TempData["detailError"] = "Detay Mevcut, Yönlendirildiğiniz Güncelleme sayfasından değişiklik yapabilirsiniz ! (Bu Sayfa)";
                return RedirectToAction("UpdateProductDetail", "ProductDetail", new { area = "Admin", id = createProductDto.ProductId });
            }
            else
            {
                await _productDetailService.CreateProductDetailAsync(createProductDto);
                return RedirectToAction("ProductListWithCategory", "Product", new { area = "Admin" });
            }
        }
    }
}
