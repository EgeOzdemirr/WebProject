using Microsoft.AspNetCore.Mvc;
using WebProject.DtoLayer.BasketDtos;
using WebProject.WebUI.Services.Interfaces;

namespace WebProject.WebUI.Services.BasketServices
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
        public BasketService(HttpClient httpClient, IUserService userService)
        {
            _httpClient = httpClient;
            _userService = userService;
        }
        [HttpPost]
        public async Task AddBasketItem(BasketItemDto basketItemDto)
        {
            var values = await GetBasket(basketItemDto);

            if (values != null)
            {
                if (!values.BasketItems.Any(x => x.ProductId == basketItemDto.ProductId))
                {
                    if (basketItemDto.Quantity > 0)
                    {
                        if (basketItemDto.Quantity > 20) // en fazla 20 ürün eklenebilir.
                        {
                            basketItemDto.Quantity = 20;
                        }
                        values.BasketItems.Add(basketItemDto);
                    }
                    else //sepete ilk ürün eklerken miktar 0 gelirse 1 yap.
                    {
                        basketItemDto.Quantity = 1;
                        values.BasketItems.Add(basketItemDto);
                    }
                }
                else
                {
                    //values=new BasketTotalDto(); adet güncellemesi yaparken dto'yu boşaltığı için null hatası bu satır yüzünden veriyor sildiğimizde sistem çalışıyor :) 
                    if (basketItemDto.Quantity > 0)
                    {
                        if (basketItemDto.Quantity > 20) // en fazla 20 ürün eklenebilir
                        {
                            basketItemDto.Quantity = 20;
                        }
                        var changeItem = values.BasketItems.FirstOrDefault(x => x.ProductId == basketItemDto.ProductId);
                        values.BasketItems.Remove(changeItem);
                        values.BasketItems.Add(basketItemDto);
                    }
                    else // miktarı sıfır yaptığımızda ürün sepetten burada siliniyor.
                    {
                        var changeItem = values.BasketItems.FirstOrDefault(x => x.ProductId == basketItemDto.ProductId);
                        values.BasketItems.Remove(changeItem);
                    }
                }
            }
            await SaveBasket(values);
        }
        public Task DeleteBasket(string userId)
        {
            throw new NotImplementedException();
        }
        public async Task<BasketTotalDto> GetBasket(BasketItemDto? basketItemDto)
        {
            var responseMessage = await _httpClient.GetAsync("Baskets");
            var values = await ReadBasketOrNull(responseMessage);
            if (values != null)
            {
                return values;
            }

            // Redis'te henüz sepet yok (servis 204 No Content dönüyor).
            var appUser = await _userService.GetUserInfo();

            if (basketItemDto == null)
            {
                // Sepet sayfası/view component'i sadece okuma yapıyor; boş sepet dönüyoruz.
                return new BasketTotalDto()
                {
                    UserId = appUser.Id,
                    BasketItems = new List<BasketItemDto>(),
                    DiscountCode = "-",
                    DiscountRate = 0
                };
            }

            BasketTotalDto basketTotal = new BasketTotalDto()
            {
                UserId = appUser.Id,
                BasketItems = new List<BasketItemDto>()
                {
                    new BasketItemDto()
                    {
                        ImageUrl = basketItemDto.ImageUrl,
                        Price = basketItemDto.Price,
                        ProductId = basketItemDto.ProductId,
                        ProductName = basketItemDto.ProductName,
                        Quantity = basketItemDto.Quantity,
                    },
                },
                DiscountCode = "-",
                DiscountRate = 0
            };
            await _httpClient.PostAsJsonAsync<BasketTotalDto>("Baskets", basketTotal);

            var responseMessage2 = await _httpClient.GetAsync("Baskets");
            return await ReadBasketOrNull(responseMessage2) ?? basketTotal;
        }

        // 204 No Content ve boş gövde JSON olarak parse edilemez; bu durumda null dönüyoruz.
        private static async Task<BasketTotalDto?> ReadBasketOrNull(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode ||
                responseMessage.StatusCode == System.Net.HttpStatusCode.NoContent ||
                responseMessage.Content.Headers.ContentLength == 0)
            {
                return null;
            }

            var values = await responseMessage.Content.ReadFromJsonAsync<BasketTotalDto>();
            if (values == null)
            {
                return null;
            }

            values.BasketItems ??= new List<BasketItemDto>();
            return values;
        }

        public async Task<bool> RemoveBasketItem(string productId)
        {
            var values = await GetBasket(null);
            var deletedItem = values.BasketItems.FirstOrDefault(x => x.ProductId == productId);
            var result = values.BasketItems.Remove(deletedItem);
            await SaveBasket(values);
            return true;
        }
        public async Task SaveBasket(BasketTotalDto basketTotalDto)
        {
            await _httpClient.PostAsJsonAsync<BasketTotalDto>("Baskets", basketTotalDto);
        }

        [HttpPost]
        public async Task<bool> UpdateQuantity(string productId, int quantity)
        {
                var values = await GetBasket(null);
                if (values.BasketItems.Any(x => x.ProductId == productId))
                {
                    var changeItem = values.BasketItems.FirstOrDefault(x => x.ProductId == productId);
                    values.BasketItems.Remove(changeItem);
                    values.BasketItems.Add(new BasketItemDto
                    {
                        ProductId = productId,
                        Quantity = Math.Min(quantity, 20) // Maksimum 20 adet eklenebilir.
                    });

                    await SaveBasket(values);
                    return true;
                }
                return false;
        }
    }
}