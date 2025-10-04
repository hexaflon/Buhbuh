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

namespace ProjektInzynierski.Pages.Group
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
      public Grupy Grupy { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Grupy == null)
            {
                return NotFound();
            }

            var grupy = await _context.Grupy.FirstOrDefaultAsync(m => m.IdGrupy == id);

            if (grupy == null)
            {
                return NotFound();
            }
            else 
            {
                Grupy = grupy;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Grupy == null)
            {
                return NotFound();
            }
            var grupy = await _context.Grupy.FindAsync(id);
            
            if (grupy != null)
            {
                if (!User.IsInRole("Admin"))
                {
                    if (grupy.IdNauczyciela != _userManager.GetUserAsync(User).Result.IdOsoba) return RedirectToPage("./Index");
                }
                Grupy = grupy;
                foreach(var test in _context.Test.Where(t => t.IdGrupy == grupy.IdGrupy))
                {
                    test.IdGrupy = null;
                }

                foreach(var os in _context.Uczestnicy.Where(u => u.IdGrupy == grupy.IdGrupy))
                {
                    _context.Uczestnicy.Remove(os);
                }
                _context.Grupy.Remove(Grupy);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./List");
        }
    }
}
