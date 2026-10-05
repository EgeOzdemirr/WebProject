using Newtonsoft.Json;
using WebProject.DtoLayer.CommentDtos;

namespace WebProject.WebUI.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;

        public CommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            await _httpClient.PostAsJsonAsync<CreateCommentDto>("Comments", createCommentDto);
        }

        public async Task DeleteCommentAsync(int id)
        {
            await _httpClient.DeleteAsync("Comments/DeleteComment/" + id);
        }

        public async Task<List<ResultCommentDto>> GetAllCommentAsync()
        {
            var responseMessage = await _httpClient.GetAsync("Comments");
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsondata);
            return values;
        }

        public async Task<UpdateCommentDto> GetByIdCommentAsync(int id)
        {
            var responseMessage = await _httpClient.GetAsync("Comments/GetComment/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateCommentDto>(jsondata);
            return value;
        }

        public async Task UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            await _httpClient.PutAsJsonAsync<UpdateCommentDto>("Comments", updateCommentDto);
        }

        public async Task<List<ResultCommentDto>> GetByProductIdCommentAsync(string id)
        {

            var responseMessage = await _httpClient.GetAsync("Comments/CommentListByProductId/" + id);
            var jsondata = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCommentDto>>(jsondata);
            return values;
        }

        public async Task ChangeStatusCommentAsync(int id)
        {
            await _httpClient.GetAsync("Comments/ChangeStatusComment/" + id);
        }
    }
}
