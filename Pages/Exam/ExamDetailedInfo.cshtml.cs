using System;
using System.Collections.Generic;
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
    [Authorize]
    public class ExamDetailedInfoModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private static int idWielokrotngo;
        private readonly UserManager<Person> _userManager;
        public ExamDetailedInfoModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            idWielokrotngo = _context.QuestionType
                .Where(tp => tp.Name == "Wielokrotnego wyboru")
                .Select(tp => tp.Id).FirstOrDefault();
            _userManager = userManager;
        }

        public List<TestTest.Models.Db.Question> testQuestions { get; set; } = default!;
        public List<TestTest.Models.Db.Question> GetQuestions(int examId)
        {
            var questions = _context.QuestionList
                .Include(lp => lp.Questions)
                .ThenInclude(p => p.Answers)
                .Where(lp => lp.TestId == examId)
                .Select(lp => lp.Questions).ToList();

            return questions;
        }
        public List<TestTest.Models.Db.Answer> selectedAnswers { get; set; } = default!;
        public string IconClass { get; set; }
        public string newStyleInput;
        public string newStyleLabel;
        public Result Result { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (_context.Result != null)
            {
                Result = _context.Result
                .FirstOrDefault(r => r.Id == id);

                if (User.IsInRole("Uczen") && _context.Test
                    .Where(t => t.Id == Result.TestId)
                    .Select(t => t.IsVisible)
                    .FirstOrDefault()==false) return RedirectToPage("./Marks");

                selectedAnswers = _context.QuestionResult
                    .Where(rdp => rdp.ResultId==id)
                    .Select(rdp => rdp.Answers)
                    .ToList();
                testQuestions = GetQuestions((int)_context.Result
                    .Where(r => r.Id==id)
                    .Select(r => r.TestId).First());
                ViewData["idWielokrotnego"] = idWielokrotngo;
                
            }
            return Page();
        }
    }
}
