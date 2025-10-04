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
    [Authorize]
    public class ExamInfoModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        private readonly SignInManager<Osoba> _signInManager;

        public ExamInfoModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager, SignInManager<Osoba> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Rozwiazanie Rozwiazanie { get; set; } = default!;
        public Test Test { get; set; } = default!;
        public List<double> Oceny { get; set; } = new List<double>();
        public int OriginalTestId { get; set; } //do powrotu na stronę Details

        public void Ocena()
        {
            var division = Rozwiazanie.LiczbaPunktow / Rozwiazanie.IdTestNavigation.ListaPytan.Count();
            division *= 100;
            division %= 100;
            if (division >= 90) Oceny.Add(5);
            else if (division >= 80 && division < 90) Oceny.Add(4.5);
            else if (division >= 70 && division < 80) Oceny.Add(4);
            else if (division >= 60 && division < 70) Oceny.Add(3.5);
            else if (division >= 50 && division < 60) Oceny.Add(3);
            else Oceny.Add(2);
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] int id)
        {

            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var rozwiazanie = await _context.Rozwiazanie.FirstOrDefaultAsync(m => m.IdRozwiazanie == id);
            if (rozwiazanie == null)
            {
                return NotFound();
            }
            else
            {
                var idOsoba = _userManager.GetUserAsync(User).Result.IdOsoba;
                if (idOsoba == rozwiazanie.IdUcznia || User.IsInRole("Admin") || User.IsInRole("Nauczyciel"))
                {
                    Test = await _context.Test
                        .Include(t => t.ListaPytan)
                        .FirstOrDefaultAsync(t => t.IdTest == rozwiazanie.IdTest);
                    if (Test == null) return NotFound();
                    OriginalTestId = Test.IdTest;
                    Rozwiazanie = rozwiazanie;
                }
                else return Forbid();
                Ocena();
            }
            return Page();
        }
    }
}
