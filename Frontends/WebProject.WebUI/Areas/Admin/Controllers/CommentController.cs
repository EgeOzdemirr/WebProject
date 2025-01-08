using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebProject.DtoLayer.CommentDtos;
using WebProject.WebUI.Services.CatalogServices.ProductServices;
using WebProject.WebUI.Services.CommentServices;

namespace WebProject.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]
    [Area("Admin")]
    [Route("Admin/Comment")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IProductService _productService;
        public CommentController(ICommentService commentService, IProductService productService)
        {

            _commentService = commentService;
            _productService = productService;
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Yorumlar";
            ViewBag.v3 = "Yorum Listesi";
            ViewBag.t = "Yorum İşlemleri";

            var values = await _commentService.GetAllCommentAsync();
            return View(values);
        }
        [Route("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteCommentAsync(id);
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }
        [Route("UpdateComment/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateComment(int id)
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "Yorumlar";
            ViewBag.v3 = "Yorum Listesi";
            ViewBag.t = "Yorum İşlemleri";
            var values = await _commentService.GetByIdCommentAsync(id);
            var values2 = await _productService.GetByIdProductAsync(values.ProductId);
            ViewBag.pName = values2.ProductName;
            return View(values);
        }
        [Route("UpdateComment/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto updateCommentDto)
        {
            await _commentService.UpdateCommentAsync(updateCommentDto);
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }
        [Route("ChangeStatusComment/{id}")]
        public async Task<IActionResult> ChangeStatusComment(int id)
        {
            await _commentService.ChangeStatusCommentAsync(id);
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }
    }
}
