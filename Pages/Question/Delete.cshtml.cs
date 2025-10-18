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
using ProjektInzynierski.utils;
using ProjektInzynierski.Utils;
using TestTest.Models.Db;
using LogLevel = ProjektInzynierski.Utils.LogLevel;

namespace TestTest.Pages.Question
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        private IAppLogger _logger;
        public DeleteModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
            //Getting an Instance of Singleton
            _logger = Logger.getInstance();
            var level = LogLevelExtensions.ToLabel(LogLevel.ERROR);
            //Declaring a Decorator
            _logger = new LevelLoggerDecorator(_logger, level);
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
                //Usage of Decorator
                _logger.Log($"Nie znaleziono pytania o id: {id}");
                return NotFound();
            }
            var pytanie = await _context.Pytanie.FindAsync(id);

            
            if (pytanie != null)
            {
                if (!User.IsInRole("Admin"))
                {
                    if (pytanie.IdNauczyciela != _userManager.GetUserAsync(User).Result.IdOsoba)
                    {
                        //Usage of Decorator
                        _logger.Log($"User: {User.Identity.Name} spróbował usunąć nie swoje pytanie o id: {id}");
                        return RedirectToPage("./Index");
                    }
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
