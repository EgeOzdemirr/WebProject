using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using WebProject.DtoLayer.CatalogDtos.CategoryDtos;
using WebProject.DtoLayer.CatalogDtos.ProductDtos;
using WebProject.WebUI.Models.RapidApiViewModels.ECommerceViewModels;
using WebProject.WebUI.Services.CatalogServices.CategoryServices;
using WebProject.WebUI.Services.CatalogServices.ProductServices;

namespace WebProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Listesi";
            ViewBag.t = "Ürün İşlemleri";

            var values = await _productService.GetAllProductAsync();
            return View(values);
        }
        [HttpGet]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Ekle";
            ViewBag.t = "Ürün İşlemleri";
            var categories = await _categoryService.GetAllCategoryAsync();
            List<SelectListItem> categoryValues = (from x in categories
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId
                                                   }).ToList();
            categoryValues.Add(new SelectListItem { Value = "", Text = "--Seçiniz--", Selected = true });
            ViewBag.categoryValues = categoryValues;
            return View();
        }
        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            await _productService.CreateProductAsync(createProductDto);

            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }
        [HttpGet]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Güncelleme";
            ViewBag.t = "Ürün İşlemleri";
            var product = await _productService.GetByIdProductAsync(id);

            var categories = await _categoryService.GetAllCategoryAsync();
            List<SelectListItem> categoryValues = (from x in categories
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId
                                                   }).ToList();
            categoryValues.Add(new SelectListItem { Value = "", Text = "--Seçiniz--", Selected = true });
            ViewBag.CategoryValues = categoryValues;

            return View(product);
        }
        [HttpPost]
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            await _productService.UpdateProductAsync(updateProductDto);

            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }

        [HttpDelete("{id}")]
        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteProductAsync(id);

            return RedirectToAction("Index", "Product", new { area = "Admin" });
        }
        [Route("ProductListWithCategory")]
        public async Task<IActionResult> ProductListWithCategory()
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Listesi";
            ViewBag.t = "Ürün İşlemleri";

            var values = await _productService.GetProductsWithCategoryAsync();
            return View(values);
        }

        ////[Route("ECommerceProductSearch")]
        ////public async Task<IActionResult> ECommerceProductSearch(string? prName)
        ////{
        ////    ViewBag.v1 = "Anasayfa";
        ////    ViewBag.v2 = "Ürünler";
        ////    ViewBag.v3 = "Ürün Arama";
        ////    ViewBag.t = "Ürün Arama";
        ////    if (prName == null)
        ////    {
        ////        prName = "Kol Saati";
        ////    };
        ////    ViewBag.prName = prName;
        ////    var client = new HttpClient();
        ////    var request = new HttpRequestMessage
        ////    {
        ////        Method = HttpMethod.Get,
        ////        RequestUri = new Uri("https://real-time-amazon-data.p.rapidapi.com/search?query=" + prName.ToString() + "&page=1&country=TR&sort_by=RELEVANCE&product_condition=ALL&is_prime=false"),
        ////        Headers =
        ////        {
        ////            { "x-rapidapi-key", "abedb3b754msh1231493457e791dp16ec12jsndbe4630f6b52" },
        ////            { "x-rapidapi-host", "real-time-amazon-data.p.rapidapi.com" },
        ////        },
        ////    };

        ////    using (var response = await client.SendAsync(request))
        ////    {
        ////        if (response.IsSuccessStatusCode)
        ////        {


        ////            response.EnsureSuccessStatusCode();
        ////            var body = await response.Content.ReadAsStringAsync();
        ////            var model = JsonConvert.DeserializeObject<ECommerceViewModel.Rootobject>(body);
        ////            var v = model.data.products.ToList();

        ////            return View(v);
        ////        }
        ////        else
        ////        {
        ////            List<ECommerceViewModel.Product>? em = new List<ECommerceViewModel.Product>();
        ////            ECommerceViewModel.Product m = new ECommerceViewModel.Product();
        ////            m.product_title = "Limit Doldu";

        ////            em.Add(m);

        ////            return View(em);
        ////        }
        ////    }
        ////}
        ////void ProductViewBagList()
        ////{
        ////    ViewBag.v1 = "Anasayfa";
        ////    ViewBag.v2 = "Kategoriler";
        ////    ViewBag.v3 = "Kategori Listesi";
        ////    ViewBag.t = "Kategori İşlemleri";
        ////}
    }
}
