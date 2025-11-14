using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class typyModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;

        public typyModel(TestTest.Models.Db.DatabaseContext context)
        {
            _context = context;
        }

        public IList<QuestionType> TypPytania { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.QuestionType != null)
            {
                TypPytania = await _context.QuestionType.ToListAsync();
            }
        }
    }
}
