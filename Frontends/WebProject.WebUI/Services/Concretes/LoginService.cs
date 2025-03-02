﻿using System.Security.Claims;
using WebProject.WebUI.Services.Interfaces;

namespace WebProject.WebUI.Services.Concretes
{
    public class LoginService : ILoginService
    {
        IHttpContextAccessor _httpContextAccessor;

        public LoginService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
