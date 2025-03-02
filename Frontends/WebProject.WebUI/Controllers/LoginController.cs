﻿//using AspNetCore;
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
            await _identityService.SignIn(signInDto);
            return RedirectToAction("Index", "Default");
        }
    }
}
