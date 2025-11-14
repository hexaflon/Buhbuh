using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Utils;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;
        private IAppLogger _logger;

        public DeleteModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = Logger.getInstance();
            var level = LogLevelExtensions.ToLabel(Utils.LogLevel.INFO);
            _logger = new LevelLoggerDecorator(_logger, level);
        }

        [BindProperty]
      public TestTest.Models.Db.Group Group { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Group == null)
            {
                return NotFound();
            }

            var group = await _context.Group.FirstOrDefaultAsync(m => m.Id == id);

            if (group == null)
            {
                return NotFound();
            }
            else 
            {
                Group = group;
            }
            return Page();
        }

         public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var group = await _context.Group.FindAsync(id);
            if (group == null)
                return NotFound();

            //użycie command
            var deleteCmd = new DeleteGroupCommand(_context, id.Value);
            var invoker = new GroupInvoker();
            invoker.AddCommand(deleteCmd);
            invoker.Run();


            _logger.Log($"User: {User.Identity.Name} usunął grupę {group.Name}");
            return RedirectToPage("./List");
        }
    }
}
