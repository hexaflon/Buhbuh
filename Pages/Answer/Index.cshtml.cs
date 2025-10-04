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

        public IList<Odpowiedz> Odpowiedzi { get; set; } = default!;
        public IList<Pytanie> Pytania { get; set; } = default!;

        public async Task OnGetAsync()
        {
            IQueryable<Odpowiedz> query = _context.Odpowiedz
                .Include(o => o.IdPytanieNavigation);

            var correctnessFilter = Request.Query["correctnessFilter"];
            var questionFilter = Request.Query["questionFilter"];

            if (!string.IsNullOrEmpty(questionFilter) && int.TryParse(questionFilter, out int questionId))
            {
                query = query.Where(o => o.IdPytanie == questionId);
            }

            Odpowiedzi = await query.ToListAsync();

            Pytania = await _context.Pytanie.ToListAsync();
        }

    }
}
