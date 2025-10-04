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

namespace TestTest.Pages.Question
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
      public Pytanie Pytanie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Pytanie == null)
            {
                return NotFound();
            }

            var pytanie = await _context.Pytanie.FirstOrDefaultAsync(m => m.IdPytanie == id);

            if (pytanie == null)
            {
                return NotFound();
            }
            else 
            {
                Pytanie = pytanie;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Pytanie == null)
            {
                return NotFound();
            }
            var pytanie = await _context.Pytanie.FindAsync(id);

            
            if (pytanie != null)
            {
                if (!User.IsInRole("Admin"))
                {
                    if (pytanie.IdNauczyciela != _userManager.GetUserAsync(User).Result.IdOsoba) return RedirectToPage("./Index");
                }
                Pytanie = pytanie;

                var odpowiedzi = _context.Odpowiedz.Where(o => o.IdPytanie == pytanie.IdPytanie).ToList();
                foreach (var odp in odpowiedzi)
                {
                    foreach(var rozDP in _context.RozwiazanieDoPytan.Where(rdp=> rdp.IdOdpowiedz == odp.IdOdpowiedz))
                    {
                        _context.RozwiazanieDoPytan.Remove(rozDP);
                    }
                    _context.Odpowiedz.Remove(odp);
                }
                foreach(var lp in _context.ListaPytan.Where(l => l.IdPytanie == pytanie.IdPytanie))
                {
                    _context.ListaPytan.Remove(lp);
                }

                _context.Pytanie.Remove(Pytanie);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
