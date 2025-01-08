using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebProject.RapidApiWebUI.Models;

namespace WebProject.RapidApiWebUI.Controllers
{
    public class DefaultController : Controller
    {
        public async Task<IActionResult> WeatherDetail()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://yahoo-weather5.p.rapidapi.com/weather?location=kocaeli&format=json&u=c"),
                Headers =
    {
        { "x-rapidapi-key", "b2bcbe13e8mshcb1461f51bf44bfp1e465djsnac765ff8a769" },
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