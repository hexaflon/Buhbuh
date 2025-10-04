using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TestTest.Models.Db;

namespace TestTest.Services
{
    public interface IUserService
    {
        Task<Osoba> GetUserAsync(ClaimsPrincipal principal);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<Osoba> _userManager;

        public UserService(UserManager<Osoba> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Osoba> GetUserAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            return user;
        }
    }

}
