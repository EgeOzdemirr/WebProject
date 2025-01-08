using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebProject.DtoLayer.CatalogDtos.ProductImageDtos;
using WebProject.WebUI.Services.CatalogServices.ProductImageServices;
using WebProject.WebUI.Services.CatalogServices.ProductServices;

namespace WebProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    [Route("Admin/ProductImage")]
    public class ProductImageController : Controller
    {
        private readonly IProductImageService _productImageService;
        private readonly IProductService _productService;

        public ProductImageController(IProductImageService productImageService, IProductService productService)
        {
            _productImageService = productImageService;
            _productService = productService;
        }

        [Route("ProductImageDetail/{id}")]
        public async Task<IActionResult> ProductImageDetail(string id)
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Görselleri";
            ViewBag.t = "Ürün Görsel İşlemleri";

            var values2 = await _productService.GetByIdProductAsync(id);

            ViewBag.pName = values2.ProductName;
            ViewBag.pId = values2.ProductId;

            var values = await _productImageService.GetByProductIdProductImageAsync(id);
            return View(values);
        }

        [Route("CreateProductImage/{id}")]
        [HttpGet]
        public async Task<IActionResult> CreateProductImage(string id)
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Görselleri";
            ViewBag.t = "Ürün Görseli Ekleme Sayfası";

            var values2 = await _productService.GetByIdProductAsync(id);

            ViewBag.pName = values2.ProductName;
            ViewBag.pId = values2.ProductId;

            return View();
        }
        [Route("CreateProductImage")]
        [HttpPost]
        public async Task<IActionResult> CreateProductImage(CreateProductImageDto createProductImageDto)
        {
            await _productImageService.CreateProductImageAsync(createProductImageDto);
            return RedirectToAction("ProductImageDetail", "ProductImage", new { area = "Admin", id = createProductImageDto.ProductId });
        }
        [Route("UpdateProductImage/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateProductImage(string id)
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Görselleri";
            ViewBag.t = "Ürün Görseli Güncelleme Sayfası";

            var values2 = await _productImageService.GetByIdProductImageAsync(id);

            var pID = values2.ProductId;
            var value = await _productService.GetByIdProductAsync(pID);
            ViewBag.pName = value.ProductName;

            return View(values2);
        }
        [Route("UpdateProductImage")]
        [HttpPost]
        public async Task<IActionResult> UpdateProductImage(UpdateProductImageDto updateProductImageDto)
        {
            await _productImageService.UpdateProductImageAsync(updateProductImageDto);
            return RedirectToAction("ProductImageDetail", "ProductImage", new { area = "Admin", id = updateProductImageDto.ProductId });
        }
        [Route("DeleteProductImage/{id}")]
        [Route("DeleteProductImage")]
        public async Task<IActionResult> DeleteProductImage(string id)
        {
            var pId = "";
            var value2 = await _productImageService.GetByIdProductImageAsync(id);
            pId = value2.ProductId;

            await _productImageService.DeleteProductImageAsync(id);
            return RedirectToAction("ProductImageDetail", "ProductImage", new { area = "Admin", id = pId });
        }
    }
}
