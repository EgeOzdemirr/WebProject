namespace WebProject.WebUI.Services.WishlistServices
{
    public interface IWishlistService
    {
        /// <summary>Giriş yapmış kullanıcının favori ürün id'leri. Hata olursa boş liste döner.</summary>
        Task<List<string>> GetIdsAsync();
        /// <summary>Favorilere ekler ya da çıkarır; eklendiyse true döner.</summary>
        Task<bool> ToggleAsync(string productId);
        Task RemoveAsync(string productId);
    }
}
