using Microsoft.AspNetCore.Mvc;
using WebProject.WebUI.Services.CommentServices;
using WebProject.WebUI.Services.StatisticServices.CatalogStatisticServices;
using WebProject.WebUI.Services.StatisticServices.CommentStatisticServices;
using WebProject.WebUI.Services.StatisticServices.DiscountStatisticServices;
using WebProject.WebUI.Services.StatisticServices.MessageStatisticServices;
using WebProject.WebUI.Services.StatisticServices.AppUserStatisticServices;

namespace WebProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Statistic")]
    public class StatisticController : Controller
    {
        private readonly ICatalogStatisticService _catalogStatisticService;
        private readonly IAppUserStatisticService _appUserStatisticService;
        private readonly ICommentStatisticService _commentStatisticService;
        private readonly IDiscountStatisticService _discountStatisticService;
        private readonly IMessageStatisticService _messageStatisticService;

        public StatisticController(ICatalogStatisticService catalogStatisticService, IAppUserStatisticService appUserStatisticService, ICommentStatisticService commentStatisticService, IDiscountStatisticService discountStatisticService, IMessageStatisticService messageStatisticService)
        {
            _catalogStatisticService = catalogStatisticService;
            _appUserStatisticService = appUserStatisticService;
            _commentStatisticService = commentStatisticService;
            _discountStatisticService = discountStatisticService;
            _messageStatisticService = messageStatisticService;
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Anasayfa";
            ViewBag.v2 = "İstatistikler";
            ViewBag.v3 = "İstatistik Sayfası";
            ViewBag.t = "İstatistik Sayfası";
            var brandsCount = await _catalogStatisticService.GetBrandCount();
            var productsCount = await _catalogStatisticService.GetProductCount();

            var maxProductPrice = await _catalogStatisticService.GetMaxProductPrice();
            var minProductPrice = await _catalogStatisticService.GetMinProductPrice();
            //var avgProductPrice = await _catalogStatisticService.GetProductAvgPrice();
            var categoryCount = await _catalogStatisticService.GetCategoryCount();

            var appUserCount = await _appUserStatisticService.GetAppUserCount();

            var commentCount = await _commentStatisticService.GetCommentsCount();
            var activeCommentCount = await _commentStatisticService.GetActiveCommentsCount();
            var passiveCommentCount = await _commentStatisticService.GetPassiveCommentsCount();

            var discountCount = await _discountStatisticService.GetDiscountCouponsCount();
            var passiveDiscountCount = await _discountStatisticService.GetPassiveDiscountCouponsCount();
            var activeDiscountCount = await _discountStatisticService.GetActiveDiscountCouponsCount();

            var totalMessageCount = await _messageStatisticService.GetTotalMessageCount();
            var readedMessageCount = await _messageStatisticService.GetReadedMessageCount();
            var nonReadedMessageCount = await _messageStatisticService.GetNonReadedMessageCount();

            ViewBag.brandCount = brandsCount;
            ViewBag.productCount = productsCount;
            ViewBag.maxProductPrice = maxProductPrice;
            ViewBag.minProductPrice = minProductPrice;
            //ViewBag.avgProductPrice = avgProductPrice;
            ViewBag.categoryCount = categoryCount;

            ViewBag.appUserCount = appUserCount;

            ViewBag.commentCount = commentCount;
            ViewBag.activeCommentCount = activeCommentCount;
            ViewBag.passiveCommentCount = passiveCommentCount;

            ViewBag.discountCount = discountCount;
            ViewBag.passiveDiscountCount = passiveDiscountCount;
            ViewBag.activeDiscountCount = activeDiscountCount;

            ViewBag.totalMessageCount = totalMessageCount;
            ViewBag.readedMessageCount = readedMessageCount;
            ViewBag.nonReadedMessageCount = nonReadedMessageCount;

            return View();
        }
    }
}
