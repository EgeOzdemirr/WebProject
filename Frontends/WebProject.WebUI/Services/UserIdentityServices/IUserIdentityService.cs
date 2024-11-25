using WebProject.DtoLayer.IdentityDtos.UserDtos;

namespace WebProject.WebUI.Services.UserIdentityServices
{
    public interface IUserIdentityService
    {
        Task<List<ResultUserDto>> GetAllUserListAsync();
    }
}
