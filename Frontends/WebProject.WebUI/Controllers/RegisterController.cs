using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Options;
using WebProject.DtoLayer.IdentityDtos.RegisterDtos;
using WebProject.WebUI.Settings;

namespace WebProject.WebUI.Controllers
{
	[AllowAnonymous]
	public class RegisterController : Controller
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServiceApiSettings _serviceApiSettings;
        public RegisterController(IHttpClientFactory httpClientFactory, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClientFactory = httpClientFactory;
            _serviceApiSettings = serviceApiSettings.Value;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(CreateRegisterDto createRegisterDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsondata = JsonConvert.SerializeObject(createRegisterDto);
            StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync($"{_serviceApiSettings.IdentityServerUrl.TrimEnd('/')}/api/Registers", stringContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}
