using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Exam
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class ListModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;

        public ListModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Test> Test { get; set; } = default!;
        public IList<Grupy> Groups { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string Visibility { get; set; }

        [BindProperty(SupportsGet = true)]
        public string GroupName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Test
                .OrderByDescending(t => t.IdTest)
                .Include(t => t.IdGrupyNavigation)
                .Include(t => t.ListaPytan)
                .ThenInclude(lp => lp.IdPytanieNavigation)
                .AsQueryable();

            //do wyszukiwania testów
            if (!string.IsNullOrEmpty(Visibility))
            {
                var visibilityValue = bool.Parse(Visibility);
                query = query.Where(t => t.CzyWidoczny == visibilityValue);
            }

            if (!string.IsNullOrEmpty(GroupName))
            {
                query = query.Where(t => t.IdGrupyNavigation.Nazwa == GroupName);
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                query = query
                    .Where(t => t.ListaPytan.Any(lp => EF.Functions.Like(lp.IdPytanieNavigation.Tresc, $"%{SearchText}%")));
            }

            if (User.IsInRole("Admin"))
            {
                Test = await _context.Test
                    .OrderByDescending(t => t.IdTest)
                    .Include(t => t.IdGrupyNavigation)
                    .Include(t => t.ListaPytan)
                    .ThenInclude(lp => lp.IdPytanieNavigation).ToListAsync();
            }
            else
            {
                var iduser = _userManager.GetUserAsync(User).Result.IdOsoba;
                Test = await _context.Test
                    .OrderByDescending(t => t.IdTest)
                    .Where(t => t.IdNauczyciela == iduser)
                    .Include(t => t.IdGrupyNavigation)
                    .Include(t => t.ListaPytan)
                    .ThenInclude(lp => lp.IdPytanieNavigation).ToListAsync();
            }

            Groups = await _context.Grupy.ToListAsync();
        }
    }
}
