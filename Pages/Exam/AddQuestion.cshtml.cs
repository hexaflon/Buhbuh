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

namespace TestTest.Pages.Exam
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class AddQuestionModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;

        public AddQuestionModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<QuestionCategory> QuestionCategory { get; set; } = default!;
        public IList<QuestionType> QuestionType { get; set; } = default!;

        public int? CategoryId { get; set; }
        public int? TypeId { get; set; }
        public string SearchText { get; set; }

        public IList<Models.Db.Question> Questions { get; set; } = new List<Models.Db.Question>();

        public IActionResult OnGet([FromQuery] int? id, int? categoryId, int? typeId, string searchText)
        {
            if (id.HasValue)
            {
                var testList = _context.Test.Include(t => t.Questions).ThenInclude(lp => lp.Questions).ToList();
                Test = (from test in testList
                           where test.Id == id
                           select test).FirstOrDefault();
                if (Test == null) return NotFound();
                ViewData["IdTest"] = id;
            }
            else
            {
                return RedirectToPage("/List");
            }
            var questions =  _context.Question.Include(p => p.Answers).Where(p => p.Answers.Count()!=0).ToList();
            var questionsInTest = _context.QuestionList.Where(lp => lp.TestId == id).Select(lp => lp.QuestionId).ToList();
            questions = questions.Where(p => !questionsInTest.Contains(p.Id)).ToList();

            if (categoryId.HasValue) questions = questions.Where(p => p.CategoryId == categoryId).ToList();
            if (typeId.HasValue) questions = questions.Where(p => p.TypeId == typeId).ToList();
            if (!string.IsNullOrEmpty(searchText)) questions = questions.Where(p => p.Text.Contains(searchText)).ToList();
          

            ViewData["IdPytanie"] = new SelectList(questions, "Id", "Text");
            Questions = questions;
            QuestionCategory =  _context.QuestionCategory.ToList();
            QuestionType =  _context.QuestionType.ToList();
            return Page();
        }

        [BindProperty]
        public QuestionList QuestionList { get; set; } = default!;

        public Test Test { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync([FromQuery] int? id)
        {
            if (!id.HasValue) return NotFound();
            if (!ModelState.IsValid || _context.QuestionList == null || QuestionList == null)
            {
                return Page();
            }
            if (_context.QuestionList == null) QuestionList.Id = 0;
            else
            {
                var questionList = _context.QuestionList.ToList();
                QuestionList.Id = (from lp in questionList
                                         orderby lp.Id descending
                                         select lp.Id).FirstOrDefault() + 1;

            }
            if(QuestionList.QuestionId==null) return RedirectToPage("", new { id = id });
            QuestionList.TestId = id;
            _context.QuestionList.Add(QuestionList);
            await _context.SaveChangesAsync();

            return RedirectToPage("",new { id = id });
        }
        public async Task<IActionResult> OnGetDelete([FromQuery] int? testId, [FromQuery] int? questionId)
        {

            if (testId == null || questionId == null) return NotFound();
            var questionsToRemove = await _context.QuestionList.FirstOrDefaultAsync(lp => lp.TestId == testId && lp.QuestionId == questionId);

            if (questionsToRemove != null)
            {
                _context.QuestionList.Remove(questionsToRemove);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("", new { id = testId });
        }
    }
}
