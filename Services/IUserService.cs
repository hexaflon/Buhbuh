using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TestTest.Models.Db;

namespace TestTest.Services
{
    public interface IUserService
    {
        Task<Person> GetUserAsync(ClaimsPrincipal principal);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<Person> _userManager;

        public UserService(UserManager<Person> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Person> GetUserAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            return user;
        }
    }

}
