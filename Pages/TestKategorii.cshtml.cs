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

namespace TestTest.Pages
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class TestKategoriiModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;

        public TestKategoriiModel(TestTest.Models.Db.DatabaseContext context)
        {
            _context = context;
        }

        public IList<KategoriaPytania> KategoriaPytania { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.KategoriaPytania != null)
            {
                KategoriaPytania = await _context.KategoriaPytania.ToListAsync();
            }
        }
    }
}
