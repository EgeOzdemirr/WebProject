using WebProject.WebUI.Models;

namespace WebProject.WebUI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDetailViewModel> GetUserInfo();
        Task<List<AllUserViewModel>> GetAllUserInfo();
    }
}
