﻿using WebProject.DtoLayer.CatalogDtos.FeatureDtos;

namespace WebProject.WebUI.Services.CatalogServices.FeatureServices
{
    public class FeatureService:IFeatureService
    {
        private readonly HttpClient _httpClient;
        public FeatureService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateFeatureAsync(CreateFeatureDto createFeatureDto)
        {
            await _httpClient.PostAsJsonAsync<CreateFeatureDto>("features", createFeatureDto);
        }

        public async Task DeleteFeatureAsync(string id)
        {
            await _httpClient.DeleteAsync("features?id=" + id);
        }

        public async Task<List<ResultFeatureDto>> GetAllFeatureAsync()
        {
            var responseMessage = await _httpClient.GetAsync("features");
            var values = await responseMessage.Content.ReadFromJsonAsync<List<ResultFeatureDto>>();
            return values;
        }

        public async Task<UpdateFeatureDto> GetByIdFeatureAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("features/" + id);
            var values = await responseMessage.Content.ReadFromJsonAsync<UpdateFeatureDto>();
            return values;

        }

        public async Task UpdateFeatureAsync(UpdateFeatureDto updateFeatureDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateFeatureDto>("features", updateFeatureDto);
        }
    }
}
