﻿
using Newtonsoft.Json;
using WebProject.DtoLayer.IdentityDtos.UserDtos;

namespace WebProject.WebUI.Services.StatisticServices.AppUserStatisticServices
{
    public class AppUserStatisticService : IAppUserStatisticService
    {
        private readonly HttpClient _httpClient;
        public AppUserStatisticService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> GetAppUserCount()
        {
            var userCount = await _httpClient.GetAsync("/api/Statistics/GetUserCount");
            var jsondata = await userCount.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<int>(jsondata);
            return value;
        }
    }
}
