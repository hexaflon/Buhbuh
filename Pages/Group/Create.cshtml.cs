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
using ProjektInzynierski.Utils;
using TestTest.Models.Db;
using LogLevel = ProjektInzynierski.Utils.LogLevel;

namespace ProjektInzynierski.Pages.Group
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class CreateModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;
        private IAppLogger _logger;
        public CreateModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = Logger.getInstance();
            var level = LogLevelExtensions.ToLabel(LogLevel.INFO);
            _logger = new LevelLoggerDecorator(_logger, level);
        }

        public IActionResult OnGet()
        {
        
            //ViewData["IdNauczyciela"] = new SelectList(_context.Osoba.Where(o => o.StatusNavigation.Nazwa.ToLower().Contains("uczen")), "IdOsoba", "imie" + " " + "nazwisko" + "email");
            return Page();
        }

        [BindProperty]
        public TestTest.Models.Db.Group Group { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Group == null || Group == null)
            {
                return Page();
            }


            var userId = _userManager.GetUserAsync(User).Result.PersonId;
            //użycie command
            var createCmd = new CreateGroupCommand(_context, Group.Name, userId); 
            var invoker = new GroupInvoker();
            invoker.AddCommand(createCmd);
            invoker.Run(); 

            var newGroup = createCmd.Result;

            _logger.Log($"User: {User.Identity.Name} utworzył grupę {newGroup.Name}");

            return RedirectToPage("./AddMembers", new { id = newGroup.Id });
        }
    }
}
