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
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        public DeleteModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
      public Test Test { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var test = await _context.Test.FirstOrDefaultAsync(m => m.IdTest == id);

            if (test == null)
            {
                return NotFound();
            }
            else 
            {
                Test = test;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }
            var test = await _context.Test.FindAsync(id);

            if (test != null)
            {
                if (!User.IsInRole("Admin"))
                {
                    if (test.IdNauczyciela != _userManager.GetUserAsync(User).Result.IdOsoba) return RedirectToPage("./List");
                }
                Test = test;
                foreach(var lp in _context.ListaPytan.Where(l => l.IdTest == test.IdTest))
                {
                    _context.ListaPytan.Remove(lp);
                }
                var rozwiazania = _context.Rozwiazanie.Where(r => r.IdTest == test.IdTest).ToList();
                foreach (var roz in rozwiazania)
                {
                    foreach(var rozDP in _context.RozwiazanieDoPytan.Where(rdp => rdp.IdRozwiazanie == roz.IdRozwiazanie))
                    {
                        _context.RozwiazanieDoPytan.Remove(rozDP);
                    }
                    _context.Rozwiazanie.Remove(roz);
                }
                _context.Test.Remove(Test);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./List");
        }
    }
}
