using WebProject.WebUI.Models;
using WebProject.WebUI.Services.Interfaces;

namespace WebProject.WebUI.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<AllUserViewModel>> GetAllUserInfo()
        {
            return await _httpClient.GetFromJsonAsync<List<AllUserViewModel>>("/api/appUsers/userlist");
        }
        public async Task<UserDetailViewModel> GetUserInfo()
        {
            return await _httpClient.GetFromJsonAsync<UserDetailViewModel>("/api/appUsers/getuser");
        }
    }
}
