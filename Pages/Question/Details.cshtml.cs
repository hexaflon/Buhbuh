using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace TestTest.Pages.Question
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class DetailsModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;

        public DetailsModel(TestTest.Models.Db.DatabaseContext context)
        {
            _context = context;
        }

      public Pytanie Pytanie { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Pytanie == null)
            {
                return NotFound();
            }

            var pytanie = await _context.Pytanie.FirstOrDefaultAsync(m => m.IdPytanie == id);
            if (pytanie == null)
            {
                return NotFound();
            }
            else 
            {
                Pytanie = pytanie;
            }
            return Page();
        }
    }
}
