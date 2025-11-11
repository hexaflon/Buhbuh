using System;
using System.Collections.Generic;
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

namespace ProjektInzynierski.Pages.Group
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
            _logger = Logger.getInstance();
            var level = LogLevelExtensions.ToLabel(Utils.LogLevel.INFO);
            _logger = new LevelLoggerDecorator(_logger, level);
        }

        [BindProperty]
      public Grupy Grupy { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Grupy == null)
            {
                return NotFound();
            }

            var grupy = await _context.Grupy.FirstOrDefaultAsync(m => m.IdGrupy == id);

            if (grupy == null)
            {
                return NotFound();
            }
            else 
            {
                Grupy = grupy;
            }
            return Page();
        }

         public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var grupy = await _context.Grupy.FindAsync(id);
            if (grupy == null)
                return NotFound();

            //użycie command
            var deleteCmd = new DeleteGroupCommand(_context, id.Value);
            var invoker = new GroupInvoker();
            invoker.AddCommand(deleteCmd);
            invoker.Run();


            _logger.Log($"User: {User.Identity.Name} usunął grupę {grupy.Nazwa}");
            return RedirectToPage("./List");
        }
    }
}
