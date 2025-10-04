using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace TestTest.Pages.Answer
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class EditModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        public EditModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Odpowiedz Odpowiedzi { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync([FromQuery]int? id)
        {
            if (id == null || _context.Odpowiedz == null)
            {
                return NotFound();
            }            
            var odpowiedzi =  await _context.Odpowiedz
                .FirstOrDefaultAsync(m => m.IdOdpowiedz == id);
            if (odpowiedzi == null)
            {
                return NotFound();
            }
            Odpowiedzi = odpowiedzi;
            ViewData["IdPytanie"] = odpowiedzi.IdPytanie;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!User.IsInRole("Admin"))
            {
                var idNauczyciela = _context.Pytanie
                .Where(p => p.IdPytanie == Odpowiedzi.IdPytanie)
                .Select(p => p.IdNauczyciela).FirstOrDefault();
                var idOsoba = _userManager.GetUserAsync(User).Result.IdOsoba;
                if (idNauczyciela != idOsoba) return RedirectToPage("./Index");
            }
            
            _context.Attach(Odpowiedzi).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OdpowiedziExists(Odpowiedzi.IdOdpowiedz))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OdpowiedziExists(int id)
        {
          return (_context.Odpowiedz?.Any(e => e.IdOdpowiedz == id)).GetValueOrDefault();
        }
    }
}
