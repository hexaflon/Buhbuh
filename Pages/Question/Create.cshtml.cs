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
using ProjektInzynierski.Pages.Answer;
using ProjektInzynierski.utils;
using TestTest.Models.Db;

namespace TestTest.Pages.Question
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class CreateModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        public readonly int IdTrueFalse;
        private Logger _logger;
        public CreateModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
            IdTrueFalse = _context.TypPytania
                .Where(tp => tp.Nazwa.ToLower().Contains("prawda"))
                .Select(tp => tp.IdTypPytania).First();
            Pytanie = new Pytanie();
            Pytanie.IdTypPytania = 1;
            _logger = Logger.getInstance();
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
            var trescOdpowiedziList = new List<String> { "Prawda","Fałsz"};

            for(int i =1; i <= 2; i++)
            {
                var odp = AnswerFactory.Create(
                    idPytanie: Pytanie.IdPytanie,
                    trescOdpowiedzi: trescOdpowiedziList.ElementAt(i-1),
                    czyPoprawny: isTrueFalse,
                    idOdpowiedz:idOdp
                    );

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
            _logger.Log($"User: {User.Identity.Name} utworzyl Pytanie {Pytanie.ToString()}");
            return RedirectToPage("./Index");
        }
    }
}
