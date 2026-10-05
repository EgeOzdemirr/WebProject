using Microsoft.AspNetCore.Mvc;
using WebProject.Basket.LoginServices;
using WebProject.Basket.Services;

namespace WebProject.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        private readonly ILoginService _loginService;

        public WishlistsController(IWishlistService wishlistService, ILoginService loginService)
        {
            _wishlistService = wishlistService;
            _loginService = loginService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyWishlist()
        {
            return Ok(await _wishlistService.GetWishlist(_loginService.GetUserId));
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> Toggle(string productId)
        {
            var added = await _wishlistService.Toggle(_loginService.GetUserId, productId);
            return Ok(new { added });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(string productId)
        {
            await _wishlistService.Remove(_loginService.GetUserId, productId);
            return Ok();
        }
    }
}
