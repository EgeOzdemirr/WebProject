using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebProject.RapidApiWebUI.Models;

namespace WebProject.RapidApiWebUI.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IConfiguration _configuration;

        public DefaultController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> WeatherDetail()
        {
            var apiKey = _configuration["RapidApi:Key"];
            if (string.IsNullOrEmpty(apiKey))
            {
                ViewBag.temperature = null;
                return View();
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://yahoo-weather5.p.rapidapi.com/weather?location=kocaeli&format=json&u=c"),
                Headers =
    {
        { "x-rapidapi-key", apiKey },
        { "x-rapidapi-host", "yahoo-weather5.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<WeatherViewModel>(body);
                var temperature = values.current_observation.condition.temperature;
                ViewBag.temperature = temperature;
                return View();
            }
        }
    }
}