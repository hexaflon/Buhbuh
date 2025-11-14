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
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace TestTest.Pages.Answer
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class EditModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;
        public EditModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Models.Db.Answer Answers { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync([FromQuery]int? id)
        {
            if (id == null || _context.Answer == null)
            {
                return NotFound();
            }            
            var answers =  await _context.Answer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answers == null)
            {
                return NotFound();
            }
            Answers = answers;
            ViewData["QuestionId"] = answers.QuestionId;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!User.IsInRole("Admin"))
            {
                var teacherId = _context.Question
                .Where(p => p.Id == Answers.QuestionId)
                .Select(p => p.TeacherId).FirstOrDefault();
                var idOsoba = _userManager.GetUserAsync(User).Result.PersonId;
                if (teacherId != idOsoba) return RedirectToPage("./Index");
            }
            
            _context.Attach(Answers).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswersExists(Answers.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AnswersExists(int id)
        {
          return (_context.Answer?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
