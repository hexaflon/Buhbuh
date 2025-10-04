using System;
using System.Collections.Generic;
using System.Data;
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
    public class ExamListModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        public ExamListModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Test> Test { get;set; } = default!;
        public IList<bool> czyRozwiazany { get; set; } = new List<bool>();

        public void sprawdzCzyRozwiazany()
        {
            foreach(var t in Test)
            {
                var iducznia = _userManager.GetUserAsync(User).Result.IdOsoba;
                if (_context.Rozwiazanie
                    .Where(r => r.IdTest == t.IdTest && r.IdUcznia == iducznia).FirstOrDefault() == null) czyRozwiazany.Add(false);
                else czyRozwiazany.Add(true);

            }
        }

        public async Task OnGetAsync()
        {
            if (_context.Test != null)
            {

                if (User.IsInRole("Admin"))
                {
                    Test = _context.Test
                        .Where(t => t.DataRozpoczecia <= DateTime.Now && t.DataZakonczenia >= DateTime.Now)
                        .ToList();
                }
                else { 
                    var iducznia = _userManager.GetUserAsync(User).Result.IdOsoba;
                    var grupucznia = _context.Uczestnicy.Where(u => u.IdUcznia == iducznia)
                        .Select(u => u.IdGrupy);
                    Test = _context.Test
                    .Where(t => grupucznia.Contains(t.IdGrupy))
                    .Where(t => t.DataRozpoczecia <= DateTime.Now && t.DataZakonczenia >= DateTime.Now).ToList();
                    sprawdzCzyRozwiazany();                
                }
            }
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] int? idTest)
        {
            if (idTest == null) return NotFound();
            var test = _context.Test.FindAsync(idTest);
            if (test == null) return NotFound();

            Rozwiazanie rozwiazanie = new Rozwiazanie();
            rozwiazanie.IdUcznia = _userManager.GetUserAsync(User).Result.IdOsoba;
            rozwiazanie.IdTest = idTest;
            var rozwiazanielist = _context.Rozwiazanie.ToList();
            if (rozwiazanielist == null) rozwiazanie.IdRozwiazanie = 0;
            else
            {
                rozwiazanie.IdRozwiazanie = rozwiazanielist
                    .OrderByDescending(r => r.IdRozwiazanie)
                    .Select(r => r.IdRozwiazanie).FirstOrDefault();
            }

            _context.Rozwiazanie.Add(rozwiazanie);
            await _context.SaveChangesAsync();

            
            return RedirectToPage("ExamPerform",new {idrozwiazania = rozwiazanie.IdRozwiazanie});
        
        }
    }
}