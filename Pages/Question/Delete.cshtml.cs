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

using ProjektInzynierski.Utils;
using TestTest.Models.Db;
using LogLevel = ProjektInzynierski.Utils.LogLevel;

namespace TestTest.Pages.Question
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
            var level = LogLevelExtensions.ToLabel(LogLevel.ERROR);
            _logger = new LevelLoggerDecorator(_logger, level);
        }

        [BindProperty]
      public Models.Db.Question Question { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FirstOrDefaultAsync(m => m.Id == id);

            if (question == null)
            {
                return NotFound();
            }
            else 
            {
                Question = question;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Question == null)
            {
                _logger.Log($"Nie znaleziono pytania o id: {id}");
                return NotFound();
            }
            var question = await _context.Question.FindAsync(id);

            
            if (question != null)
            {
                if (!User.IsInRole("Admin"))
                {
                    if (question.TeacherId != _userManager.GetUserAsync(User).Result.PersonId)
                    {
                        //Użycie strategii
                        _logger.SetStrategy(new ColoredLogStrategy());
                        _logger.Log($"User: {User.Identity.Name} spróbował usunąć nie swoje pytanie o id: {id}");
                        return RedirectToPage("./Index");
                    }
                }
                Question = question;

                var answers = _context.Answer.Where(o => o.QuestionId == question.Id).ToList();
                foreach (var odp in answers)
                {
                    foreach(var rozDP in _context.QuestionResult.Where(rdp=> rdp.AnswerId == odp.Id))
                    {
                        _context.QuestionResult.Remove(rozDP);
                    }
                    _context.Answer.Remove(odp);
                }
                foreach(var lp in _context.QuestionList.Where(l => l.QuestionId == question.Id))
                {
                    _context.QuestionList.Remove(lp);
                }

                _context.Question.Remove(Question);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
