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
using TestTest.Models.Db;

namespace TestTest.Pages.Question
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class CreateModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        public readonly int IdTrueFalse;
        public CreateModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
            IdTrueFalse = _context.TypPytania
                .Where(tp => tp.Nazwa.ToLower().Contains("prawda"))
                .Select(tp => tp.IdTypPytania).First();
            Pytanie = new Pytanie();
            Pytanie.IdTypPytania = 1;
        }

        public IActionResult OnGet()
        {
            
        ViewData["IdKategoriaPytania"] = new SelectList(_context.KategoriaPytania, "IdKategoriaPytania", "Nazwa");
        ViewData["IdTypPytania"] = new SelectList(_context.TypPytania, "IdTypPytania", "Nazwa");
            
            
            return Page();
        }

        [BindProperty]
        public Pytanie Pytanie { get; set; } = default!;
        [BindProperty]
        public bool isTrueFalse { get; set; } = false;
        public void dodajOdpowiedzi()
        {
            var odpowiedzi = _context.Odpowiedz.ToList();
            int idOdp = 1;
            Console.WriteLine(isTrueFalse);
            if (odpowiedzi != null)
            {
                idOdp = odpowiedzi
                    .OrderByDescending(o => o.IdOdpowiedz)
                    .Select(o => o.IdOdpowiedz)
                    .FirstOrDefault() + 1;
            }
            for(int i = 1; i <= 2; i++)
            {
                var odp = new Odpowiedz();
                odp.IdPytanie = Pytanie.IdPytanie;
                if (i % 2 == 1)
                {
                    odp.TrescOdpowiedzi = "Prawda";
                    if (isTrueFalse) odp.CzyPoprawny = isTrueFalse;
                }
                if (i % 2 == 0)
                {
                    odp.TrescOdpowiedzi = "Fałsz";
                    if (!isTrueFalse) odp.CzyPoprawny = !isTrueFalse;
                }
                odp.IdOdpowiedz = idOdp;
                idOdp++;
                _context.Odpowiedz.Add(odp);
                _context.SaveChanges();
                
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || Pytanie == null || _context.Pytanie == null)
            {
                return Page();
            }


            int id;
            var query = _context.Pytanie.OrderByDescending(x => x.IdPytanie).FirstOrDefault();
            if (query == null) id = 0;
            else
            {
                id = query.IdPytanie + 1;
            }
            Pytanie.IdPytanie = id;
            Pytanie.IdNauczyciela = _userManager.GetUserAsync(User).Result.IdOsoba;
            _context.Pytanie.Add(Pytanie);
            await _context.SaveChangesAsync();
            if (Pytanie.IdTypPytania == IdTrueFalse)dodajOdpowiedzi();

            return RedirectToPage("./Index");
        }
    }
}
