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

namespace ProjektInzynierski.Pages.Exam
{
    [Authorize(Roles = "Uczen,Admin")]
    public class MarksModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;

        public MarksModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Rozwiazanie> Rozwiazanie { get;set; } = default!;
        public List<double> Oceny { get; set; } = new List<double>();

        public void Ocena()
        {
            foreach (var wynik in Rozwiazanie)
            {
                var division = wynik.LiczbaPunktow / wynik.IdTestNavigation.ListaPytan.Count();
                division *= 100;
                division %= 100;
                if (division >= 90) Oceny.Add(5);
                else if (division >= 80 && division < 90) Oceny.Add(4.5);
                else if (division >= 70 && division < 80) Oceny.Add(4);
                else if (division >= 60 && division < 70) Oceny.Add(3.5);
                else if (division >= 50 && division < 60) Oceny.Add(3);
                else Oceny.Add(2);
            }
        }

        public async Task OnGetAsync()
        {
            if (_context.Rozwiazanie != null)
            {
                if (User.IsInRole("Admin"))
                {
                    Rozwiazanie = await _context.Rozwiazanie
                    .Include(r => r.IdTestNavigation)
                    .ThenInclude(t=>t.ListaPytan).ToListAsync();
                }
                else
                {
                    var id = _userManager.GetUserAsync(User).Result.IdOsoba;
                    Rozwiazanie = await _context.Rozwiazanie
                    .Include(r => r.IdTestNavigation)
                    .ThenInclude(t => t.ListaPytan)
                    .Where(r => r.IdUcznia == id)
                    .ToListAsync();
                }
                Ocena();
            }
        }
    }
}
