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
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Exam
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;
        public DeleteModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
      public Test Test { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var test = await _context.Test.FirstOrDefaultAsync(m => m.Id == id);

            if (test == null)
            {
                return NotFound();
            }
            else 
            {
                Test = test;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }
            var test = await _context.Test.FindAsync(id);

            if (test != null)
            {
                if (!User.IsInRole("Admin"))
                {
                    if (test.TeacherId != _userManager.GetUserAsync(User).Result.PersonId) return RedirectToPage("./List");
                }
                Test = test;
                foreach(var lp in _context.QuestionList.Where(l => l.TestId == test.Id))
                {
                    _context.QuestionList.Remove(lp);
                }
                var results = _context.Result.Where(r => r.TestId == test.Id).ToList();
                foreach (var roz in results)
                {
                    foreach(var rozDP in _context.QuestionResult.Where(rdp => rdp.ResultId == roz.Id))
                    {
                        _context.QuestionResult.Remove(rozDP);
                    }
                    _context.Result.Remove(roz);
                }
                _context.Test.Remove(Test);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./List");
        }
    }
}
