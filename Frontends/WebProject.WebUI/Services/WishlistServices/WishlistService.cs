using Newtonsoft.Json.Linq;

namespace WebProject.WebUI.Services.WishlistServices
{
    public class WishlistService : IWishlistService
    {
        private readonly HttpClient _httpClient;

        public WishlistService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> GetIdsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Wishlists");
                if (!response.IsSuccessStatusCode)
                {
                    return new List<string>();
                }
                var json = await response.Content.ReadAsStringAsync();
                return JArray.Parse(json).Select(x => x.ToString()).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<bool> ToggleAsync(string productId)
        {
            var response = await _httpClient.PostAsync($"Wishlists/{Uri.EscapeDataString(productId)}", null);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JObject.Parse(json).Value<bool>("added");
        }

        public async Task RemoveAsync(string productId)
        {
            await _httpClient.DeleteAsync($"Wishlists/{Uri.EscapeDataString(productId)}");
        }
    }
}
