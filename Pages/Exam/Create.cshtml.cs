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
using ProjektInzynierski.utils;
using ProjektInzynierski.Utils;
using TestTest.Models.Db;
using LogLevel = ProjektInzynierski.Utils.LogLevel;

namespace ProjektInzynierski.Pages.Exam
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class CreateModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        private IAppLogger _logger;
        public CreateModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = Logger.getInstance();
            var level = LogLevelExtensions.ToLabel(LogLevel.INFO);
            _logger = new LevelLoggerDecorator(_logger, level);
        }

        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserAsync(User).Result.IdOsoba; 
            ViewData["IdGrupy"] = new SelectList(_context.Grupy.Where(g => g.IdNauczyciela==userId), "IdGrupy", "Nazwa");
            return Page();
        }

        [BindProperty]
        public Test Test { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Test == null || Test == null)
            {
                return Page();
            }
            Test.DataUtworzenia = DateTime.Now;
            //dodać dla obecnego użytkownika
            Test.IdNauczyciela = _userManager.GetUserAsync(User).Result.IdOsoba;

            var testyList = _context.Test.ToList();
            if (testyList == null) Test.IdTest = 0;
            else
            {
                Test.IdTest = testyList.OrderByDescending(test => test.IdTest).Select(test => test.IdTest).FirstOrDefault()+1;
            }

            _context.Test.Add(Test);
            await _context.SaveChangesAsync();
            _logger.Log($"User: {User.Identity.Name} utworzyl test {Test.ToString()}");
            return RedirectToPage("./List");
        }
    }
}
