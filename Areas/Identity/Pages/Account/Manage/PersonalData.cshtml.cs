using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ProjektInzynierski.utils;
using TestTest.Models.Db;

namespace ProjektInzynierski.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<Osoba> _userManager;

        public PersonalDataModel(
            UserManager<Osoba> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można wczytać użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}
