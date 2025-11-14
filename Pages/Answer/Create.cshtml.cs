using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ProjektInzynierski.Pages.Answer;
using ProjektInzynierski.Utils;
using TestTest.Models.Db;
using LogLevel = ProjektInzynierski.Utils.LogLevel;

namespace TestTest.Pages.Answer
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

        [BindProperty]
        public Models.Db.Answer Answer { get; set; }
        public Models.Db.Question VisibleQuestion { get; set; }

        public bool hasCorrectAnswer { get; set; } = false;

        public IActionResult OnGet([FromQuery] int? id)
        {
            if (id.HasValue)
            {
                ViewData["id"] = id;
                

                if (_context.Question == null) return Page();
                var QuestionList = _context.Question.Include(p => p.Answers).ToList();
                VisibleQuestion = (from pyt in QuestionList
                              where pyt.Id == id
                              select pyt).FirstOrDefault();
                if (VisibleQuestion == null)
                {
                    if (VisibleQuestion.TeacherId != _userManager.GetUserAsync(User).Result.PersonId) return Forbid();
                    return RedirectToPage("/Question/Index");
                }

                if (VisibleQuestion.TypeId != 2)
                {
                    if (VisibleQuestion.Answers.Any(o => o.IsCorrect == true)) hasCorrectAnswer = true;
                }



            }
            else
            {
                ViewData["id"] = -1;
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] int id)
        {

            var question = _context.Question.FirstOrDefault(p => p.Id == id);
            if (question == null) return NotFound();
            if (!ModelState.IsValid || _context.Answer == null || Answer == null)
            {
                return Page();
            }
            if (_context.Answer == null) Answer.Id = 0;
            else
            {
                var AnswerList = _context.Answer.ToList();
                Answer.Id = (from odp in AnswerList
                                         orderby odp.Id descending
                                         select odp.Id).FirstOrDefault() + 1;

            }
            if (id != null) Answer.QuestionId = id;


            var odpSave = AnswerFactory.Create(
                    questionId: id,
                    text: Answer.Text,
                    isCorrect: Answer.IsCorrect,
                    AnswerId: Answer.Id
                    );


            _context.Answer.Add(Answer);
            await _context.SaveChangesAsync();
            _logger.Log($"User: {User.Identity.Name} utworzyl odpowiedź {Answer.ToString()}");
            return RedirectToPage("", new { id = id });
        }

    }
}
