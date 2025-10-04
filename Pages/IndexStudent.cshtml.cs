using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace TestTest.Pages
{
    [Authorize(Roles = "Uczen,Admin")]
    public class IndexStudentModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;

        public IndexStudentModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Test> Test { get; set; } = default!;
        public IList<Rozwiazanie> Rozwiazanie { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Test != null)
            {
                if (User.IsInRole("Admin"))
                {
                    Test = _context.Test
                        .Where(t => t.DataRozpoczecia <= DateTime.Now && t.DataZakonczenia >= DateTime.Now)
                        .OrderBy(t => t.DataZakonczenia)
                        .ToList();
                }
                else
                {
                    var iducznia = _userManager.GetUserAsync(User).Result.IdOsoba;
                    var grupucznia = _context.Uczestnicy.Where(u => u.IdUcznia == iducznia)
                        .Select(u => u.IdGrupy);

                    Test = _context.Test
                        .Where(t => grupucznia.Contains(t.IdGrupy))
                        .Where(t => t.DataRozpoczecia <= DateTime.Now && t.DataZakonczenia >= DateTime.Now)
                        .OrderBy(t => t.DataZakonczenia)
                        .ToList();
                }
            }
        }
    }
}
