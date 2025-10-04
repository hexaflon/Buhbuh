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

namespace TestTest.Pages.Answer
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
      public Odpowiedz Odpowiedzi { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Odpowiedz == null)
            {
                return NotFound();
            }

            var odpowiedzi = await _context.Odpowiedz.FirstOrDefaultAsync(m => m.IdOdpowiedz == id);

            if (odpowiedzi == null)
            {
                return NotFound();
            }
            else 
            {
                Odpowiedzi = odpowiedzi;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Odpowiedz == null)
            {
                return NotFound();
            }
            var odpowiedzi = _context.Odpowiedz.Include(o => o.IdPytanieNavigation).Where(o => o.IdOdpowiedz == id).First();
            var rDPList = _context.RozwiazanieDoPytan.Where(rdp => rdp.IdOdpowiedz == id).ToList();

            foreach(var rdp in rDPList)
            {
                _context.RozwiazanieDoPytan.Remove(rdp);
                _context.SaveChanges();
            }

            if (odpowiedzi != null)
            {
                if (!User.IsInRole("Admin"))
                {
                    if (odpowiedzi.IdPytanieNavigation.IdNauczyciela != _userManager.GetUserAsync(User).Result.IdOsoba) return RedirectToPage("./Index");
                }
                Odpowiedzi = odpowiedzi;
                _context.Odpowiedz.Remove(Odpowiedzi);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Create", new { id = Odpowiedzi.IdPytanie });
            return RedirectToPage("./Index");
        }
    }
}
