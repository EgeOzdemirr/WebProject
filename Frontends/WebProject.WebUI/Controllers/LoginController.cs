//using AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WebProject.DtoLayer.IdentityDtos.LoginDtos;
using WebProject.WebUI.Models;
using WebProject.WebUI.Services.Interfaces;

namespace WebProject.WebUI.Controllers
{
    public class LoginController : Controller
	{
        private readonly IIdentityService _identityService;
        public LoginController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(SignInDto signInDto)
        {
            var signedIn = await _identityService.SignIn(signInDto);
            if (!signedIn)
            {
                ViewBag.LoginError = "Kullanıcı adı veya şifre hatalı (ya da hesap geçici olarak kilitlendi).";
                return View();
            }
            return RedirectToAction("Index", "Default");
        }
    }
}
