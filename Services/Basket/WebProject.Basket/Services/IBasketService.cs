using WebProject.Basket.Dtos;

namespace WebProject.Basket.Services
{
    public interface IBasketService
    {
        Task<BasketTotalDto> GetBasket(string userId);
        Task SaveBasket(BasketTotalDto basket);
        Task DeleteBasket(string userId);
    }
}
