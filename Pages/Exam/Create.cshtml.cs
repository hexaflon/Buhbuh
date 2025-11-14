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

namespace ProjektInzynierski.Pages.Exam
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
            var userId = _userManager.GetUserAsync(User).Result.PersonId; 
            ViewData["IdGrupy"] = new SelectList(_context.Group.Where(g => g.TeacherId==userId), "Id", "Name");
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
            Test.CreationDate = DateTime.Now;
            //dodać dla obecnego użytkownika
            Test.TeacherId = _userManager.GetUserAsync(User).Result.PersonId;

            var testList = _context.Test.ToList();
            if (testList == null) Test.Id = 0;
            else
            {
                Test.Id = testList.OrderByDescending(test => test.Id).Select(test => test.Id).FirstOrDefault()+1;
            }

            _context.Test.Add(Test);
            await _context.SaveChangesAsync();
            _logger.Log($"User: {User.Identity.Name} utworzyl test {Test.ToString()}");
            return RedirectToPage("./List");
        }
    }
}
