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
using ProjektInzynierski.Pages.Question;
using ProjektInzynierski.utils;
using ProjektInzynierski.Utils;
using TestTest.Models.Db;
using LogLevel = ProjektInzynierski.Utils.LogLevel;

namespace TestTest.Pages.Question
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class CreateModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        public readonly int IdTrueFalse;
        private IAppLogger _logger;
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
            var level = LogLevelExtensions.ToLabel(LogLevel.INFO);
            _logger = new LevelLoggerDecorator(_logger, level);
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
       
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || Pytanie == null || _context.Pytanie == null)
            {
                return Page();
            }

            Pytanie.IdNauczyciela = _userManager.GetUserAsync(User).Result?.IdOsoba;

            var facade = new QuestionFacade(_context);

            var nowePytanie = facade.CreateQuestion(
                tresc: Pytanie.Tresc,
                idNauczyciela: (int)Pytanie.IdNauczyciela,
                idKategoria: Pytanie.IdKategoriaPytania,
                idTypPytania: Pytanie.IdTypPytania ?? 1,
                isTrueFalse: isTrueFalse
                );


            _logger.Log($"User: {User.Identity.Name} utworzyl Pytanie {Pytanie.ToString()}");
            
            return RedirectToPage("./Index");
        }
    }
}
