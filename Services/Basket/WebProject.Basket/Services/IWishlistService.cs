namespace WebProject.Basket.Services
{
    public interface IWishlistService
    {
        Task<List<string>> GetWishlist(string userId);
        /// <summary>Ürün listede yoksa ekler, varsa çıkarır. Eklendiyse true döner.</summary>
        Task<bool> Toggle(string userId, string productId);
        Task Remove(string userId, string productId);
    }
}
