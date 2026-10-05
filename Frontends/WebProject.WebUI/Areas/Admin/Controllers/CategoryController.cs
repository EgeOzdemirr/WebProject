using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebProject.DtoLayer.CatalogDtos.CategoryDtos;
using WebProject.WebUI.Services.CatalogServices.CategoryServices;

namespace WebProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        void CategoryViewBagList()
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Kategoriler";
            ViewBag.v3 = "Kategori Listesi";
            ViewBag.t = "Kategori İşlemleri";
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            CategoryViewBagList();

            var values = await _categoryService.GetAllCategoryAsync();
            return View(values);
        }
        [HttpGet]
        [Route("CreateCategory")]
        public IActionResult CreateCategory()
        {
            CategoryViewBagList();
            return View();
        }
        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            await _categoryService.CreateCategoryAsync(createCategoryDto);
            var url = "/Admin/Category/Index/";
            return Redirect(url);
        }

        [Route("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            var url = "/Admin/Category/Index/";
            return Redirect(url);

        }
        [Route("UpdateCategory/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string id)
        {
            CategoryViewBagList();
            var value = await _categoryService.GetByIdCategoryAsync(id);
            //    CategoryID = id,
            //    CategoryName = value.CategoryName,
            //    ImageUrl = value.ImageUrl

            return View(value);
        }
        [Route("UpdateCategory/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            var url = "/Admin/Category/Index/";
            return Redirect(url);

        }
        [Route("GetProductsByCategoryId/{id}")]
        public async Task<IActionResult> GetProductsByCategoryId(string id)
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Ürünler";
            ViewBag.v3 = "Ürün Listesi";

            var value2 = await _categoryService.GetByIdCategoryAsync(id);
            if (value2 == null)
            {
                return NotFound();
            }

            var value = await _categoryService.GetProductsByCategoryIdAsync(id);

            ViewBag.t = value.Count > 0 ? value2.CategoryName + " " + "Kategorisindeki Ürünler" : value2.CategoryName + " " + "Kategorisinde Henüz Ürün Yok";

            return View(value);
        }
    }
}