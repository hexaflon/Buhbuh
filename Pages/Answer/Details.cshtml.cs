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

namespace TestTest.Pages.Answer
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class DetailsModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;

        public DetailsModel(TestTest.Models.Db.DatabaseContext context)
        {
            _context = context;
        }

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
    }
}
