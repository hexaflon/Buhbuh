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

namespace TestTest.Pages.Answer
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
      public Models.Db.Answer Answers { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Answer == null)
            {
                return NotFound();
            }

            var answers = await _context.Answer.FirstOrDefaultAsync(m => m.Id == id);

            if (answers == null)
            {
                return NotFound();
            }
            else 
            {
                Answers = answers;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Answer == null)
            {
                return NotFound();
            }
            var answers = _context.Answer.Include(o => o.Questions).Where(o => o.Id == id).First();
            var rDPList = _context.QuestionResult.Where(rdp => rdp.AnswerId == id).ToList();

            foreach(var rdp in rDPList)
            {
                _context.QuestionResult.Remove(rdp);
                _context.SaveChanges();
            }

            if (answers != null)
            {
                if (!User.IsInRole("Admin"))
                {
                    if (answers.Questions.TeacherId != _userManager.GetUserAsync(User).Result.PersonId) return RedirectToPage("./Index");
                }
                Answers = answers;
                _context.Answer.Remove(Answers);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Create", new { id = Answers.QuestionId });
            return RedirectToPage("./Index");
        }
    }
}
