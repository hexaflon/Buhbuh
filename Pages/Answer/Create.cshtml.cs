using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using TestTest.Models.Db;

namespace TestTest.Pages.Answer
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class CreateModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        public CreateModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public Odpowiedz Odpowiedz { get; set; }
        public Pytanie wysPytanie { get; set; }

        public bool hasCorrectAnswer { get; set; } = false;

        public IActionResult OnGet([FromQuery] int? id)
        {
            if (id.HasValue)
            {
                ViewData["id"] = id;
                

                if (_context.Pytanie == null) return Page();
                var PytanieList = _context.Pytanie.Include(p => p.Odpowiedz).ToList();
                wysPytanie = (from pyt in PytanieList
                              where pyt.IdPytanie == id
                              select pyt).FirstOrDefault();
                if (wysPytanie == null)
                {
                    if (wysPytanie.IdNauczyciela != _userManager.GetUserAsync(User).Result.IdOsoba) return Forbid();
                    return RedirectToPage("/Question/Index");
                }

                if (wysPytanie.IdTypPytania != 2)
                {
                    if (wysPytanie.Odpowiedz.Any(o => o.CzyPoprawny == true)) hasCorrectAnswer = true;
                }



            }
            else
            {
                ViewData["id"] = -1;
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] int id)
        {

            var pytanie = _context.Pytanie.FirstOrDefault(p => p.IdPytanie == id);
            if (pytanie == null) return NotFound();
            if (!ModelState.IsValid || _context.Odpowiedz == null || Odpowiedz == null)
            {
                return Page();
            }
            if (_context.Odpowiedz == null) Odpowiedz.IdOdpowiedz = 0;
            else
            {
                var OdpowiedziList = _context.Odpowiedz.ToList();
                Odpowiedz.IdOdpowiedz = (from odp in OdpowiedziList
                                         orderby odp.IdOdpowiedz descending
                                         select odp.IdOdpowiedz).FirstOrDefault() + 1;

            }
            if (id != null) Odpowiedz.IdPytanie = id;

            _context.Odpowiedz.Add(Odpowiedz);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("", new { id = id });
        }

    }
}
