using System.Text.Json;
using WebProject.Basket.Settings;

namespace WebProject.Basket.Services
{
    /// <summary>Kullanıcının favori ürün id'leri Redis'te "wishlist:{userId}" anahtarında tutulur.</summary>
    public class WishlistService : IWishlistService
    {
        private const int MaxItems = 100;
        private readonly RedisService _redisService;

        public WishlistService(RedisService redisService)
        {
            _redisService = redisService;
        }

        private static string Key(string userId) => $"wishlist:{userId}";

        public async Task<List<string>> GetWishlist(string userId)
        {
            var json = await _redisService.GetDb().StringGetAsync(Key(userId));
            if (string.IsNullOrEmpty(json))
            {
                return new List<string>();
            }
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }

        public async Task<bool> Toggle(string userId, string productId)
        {
            var list = await GetWishlist(userId);
            var added = !list.Remove(productId);
            if (added)
            {
                list.Insert(0, productId);
                if (list.Count > MaxItems)
                {
                    list = list.Take(MaxItems).ToList();
                }
            }
            await _redisService.GetDb().StringSetAsync(Key(userId), JsonSerializer.Serialize(list));
            return added;
        }

        public async Task Remove(string userId, string productId)
        {
            var list = await GetWishlist(userId);
            if (list.Remove(productId))
            {
                await _redisService.GetDb().StringSetAsync(Key(userId), JsonSerializer.Serialize(list));
            }
        }
    }
}
