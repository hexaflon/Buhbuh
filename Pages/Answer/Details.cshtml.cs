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

      public Odpowiedz Odpowiedzi { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Odpowiedz == null)
            {
                return NotFound();
            }

            var odpowiedzi = await _context.Odpowiedz.FirstOrDefaultAsync(m => m.IdOdpowiedz == id);
            if (odpowiedzi == null)
            {
                return NotFound();
            }
            else 
            {
                Odpowiedzi = odpowiedzi;
            }
            return Page();
        }
    }
}
