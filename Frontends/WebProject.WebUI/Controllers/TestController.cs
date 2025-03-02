﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using WebProject.DtoLayer.CatalogDtos.CategoryDtos;
using WebProject.WebUI.Services.CatalogServices.CategoryServices;

namespace WebProject.WebUI.Controllers
{
    public class TestController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICategoryService _categoryService;
        public TestController(IHttpClientFactory httpClientFactory, HttpClient httpClient, ICategoryService categoryService)
        {
            _httpClientFactory = httpClientFactory;
            _categoryService = categoryService;
        }
        //[Route("Index")]
        public async Task<IActionResult> Index()
        {
            //string token = "";
            //using (var httpClient = new HttpClient())
            //{
            //    var request = new HttpRequestMessage
            //    {
            //        RequestUri = new Uri("http://localhost:5001/connect/token"),
            //        Method = HttpMethod.Post,
            //        Content = new FormUrlEncodedContent(new Dictionary<string, string>
            //        {
            //            { "client_id", "MultiShopVisitorId" },
            //            { "client_secret", "multishopsecret" },
            //            { "grant_type", "client_credentials" }

            //        })
            //    };

            //    using (var response = await httpClient.SendAsync(request))
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            var content = await response.Content.ReadAsStringAsync();
            //            var tokenResponse = JObject.Parse(content);
            //            token = tokenResponse["access_token"].ToString();
            //        }
            //    }
            //}

            //var client = _httpClientFactory.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //var responseMessage = await client.GetAsync("https://localhost:7070/api/Categories");
            //if (responseMessage.IsSuccessStatusCode)
            //{
            //    var jsondata = await responseMessage.Content.ReadAsStringAsync();
            //    var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsondata);
            //    return View(values);
            //}
            return View();
        }
        public IActionResult Deneme1()
        {
            return View();
        }
        public async Task<IActionResult> Deneme2()
        {
            var values = await _categoryService.GetAllCategoryAsync();
            return View(values);
        }
    }
}
