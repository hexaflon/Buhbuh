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

namespace ProjektInzynierski.Pages.Group
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
            //Getting an Instance of Singleton
            _logger = Logger.getInstance();
            var level = LogLevelExtensions.ToLabel(LogLevel.INFO);
            //Declaring a Decorator
            _logger = new LevelLoggerDecorator(_logger, level);
        }

        public IActionResult OnGet()
        {
        
            //ViewData["IdNauczyciela"] = new SelectList(_context.Osoba.Where(o => o.StatusNavigation.Nazwa.ToLower().Contains("uczen")), "IdOsoba", "imie" + " " + "nazwisko" + "email");
            return Page();
        }

        [BindProperty]
        public Grupy Grupy { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Grupy == null || Grupy == null)
            {
                return Page();
            }

            var grupyList = _context.Grupy.ToList();

            int idgrupy = 0;

            if (grupyList == null) idgrupy = 0;
            else
            {
                idgrupy = grupyList.OrderByDescending(gr => gr.IdGrupy).Select(gr => gr.IdGrupy).FirstOrDefault()+1;
            }
            //Declaring a Builder
            var builder = new GroupBuilder();
            //Usage of Builder
            var grupa = builder.SetNazwa(Grupy.Nazwa)
                .SetNauczyciel(_userManager.GetUserAsync(User).Result.IdOsoba)
                .SetID(idgrupy)
                .Build();
            
            _context.Grupy.Add(grupa);
            await _context.SaveChangesAsync();
            //Usage of Decorator
            _logger.Log($"User: {User.Identity.Name} utworzyl Grupę {grupa.ToString()}");
            return RedirectToPage("./AddMembers", new {id = Grupy.IdGrupy});
        }
    }
}
