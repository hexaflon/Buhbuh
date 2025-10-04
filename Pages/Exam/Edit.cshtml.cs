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

namespace ProjektInzynierski.Pages.Exam
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
        public Test Test { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var test =  await _context.Test.FirstOrDefaultAsync(m => m.IdTest == id);
            if (test == null)
            {
                return NotFound();
            }
            Test = test;
            ViewData["IdGrupy"] = test.IdGrupy;
            ViewData["IdNauczyciela"] = test.IdNauczyciela;
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
                if (Test.IdNauczyciela != _userManager.GetUserAsync(User).Result.IdOsoba) return RedirectToPage("./List");
            }
            _context.Attach(Test).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(Test.IdTest))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./List");
        }

        private bool TestExists(int id)
        {
          return (_context.Test?.Any(e => e.IdTest == id)).GetValueOrDefault();
        }
    }
}
