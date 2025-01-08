using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WebProject.IdentityServer.Dtos;
using WebProject.IdentityServer.Models;
using static IdentityServer4.IdentityServerConstants;


namespace WebProject.IdentityServer.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AppUsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "User registered successfully" });
        }

        [Authorize(LocalApi.PolicyName)]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            var user = await _userManager.FindByIdAsync(userClaim.Value);
            return Ok(new
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                UserName = user.UserName
            });
        }

        [Authorize(LocalApi.PolicyName)]
        [HttpGet("UserList")]
        public async Task<IActionResult> UserList()
        {
            var userList = await _userManager.Users.ToListAsync();
            List<AppUserListDto> modelUsers = new List<AppUserListDto>();
            foreach (var user in userList)
            {
                AppUserListDto mUser = new AppUserListDto()
                {
                    Email = user.Email,
                    Name = user.Name,
                    Surname = user.Surname,
                    Id = user.Id,
                    UserName = user.UserName
                };
                modelUsers.Add(mUser);
            }
            return Ok(modelUsers);
        }
    }
}
