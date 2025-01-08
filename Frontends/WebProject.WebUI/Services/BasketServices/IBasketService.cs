using WebProject.DtoLayer.BasketDtos;

namespace WebProject.WebUI.Services.BasketServices
{
    public interface IBasketService
    {
        Task<BasketTotalDto> GetBasket(BasketItemDto? basketItemDto);
        Task SaveBasket(BasketTotalDto basketTotalDto);
        Task DeleteBasket(string userId);
        Task AddBasketItem(BasketItemDto basketItemDto);
        Task<bool> UpdateQuantity(string productId, int quantity);
        Task<bool> RemoveBasketItem(string productId);
    }
}
