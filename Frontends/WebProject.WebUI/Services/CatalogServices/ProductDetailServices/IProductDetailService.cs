﻿using WebProject.DtoLayer.CatalogDtos.ProductDetailDtos;

namespace WebProject.WebUI.Services.CatalogServices.ProductDetailServices
{
    public interface IProductDetailService
    {
        Task<List<ResultProductDetailDto>> GetAllProductDetailAsync();
        Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto);
        Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto);
        Task DeleteProductDetailAsync(string id);
        Task<UpdateProductDetailDto> GetByIdProductDetailAsync(string id);
        Task<UpdateProductDetailDto> GetByProductIdProductDetailAsync(string id);
    }
}
