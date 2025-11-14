using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace TestTest.Pages.Answer
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class IndexModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;

        public IndexModel(TestTest.Models.Db.DatabaseContext context)
        {
            _context = context;
        }

        public IList<Models.Db.Answer> Answers { get; set; } = default!;
        public IList<Models.Db.Question> Questions { get; set; } = default!;

        public async Task OnGetAsync()
        {
            IQueryable<Models.Db.Answer> query = _context.Answer
                .Include<Models.Db.Answer, Models.Db.Question>((System.Linq.Expressions.Expression<Func<Models.Db.Answer, Models.Db.Question?>>)(o => o.Questions));

            var correctnessFilter = Request.Query["correctnessFilter"];
            var questionFilter = Request.Query["questionFilter"];

            if (!string.IsNullOrEmpty(questionFilter) && int.TryParse(questionFilter, out int questionId))
            {
                query = query.Where(o => o.QuestionId == questionId);
            }

            Answers = await query.ToListAsync();

            Questions = await _context.Question.ToListAsync();
        }

    }
}
