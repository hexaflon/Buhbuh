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

namespace TestTest.Pages
{
    [Authorize(Roles = "Nauczyciel, Admin")]
    public class IndexTeacherModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly ILogger<IndexTeacherModel> _logger;
        private readonly UserManager<Osoba> _userManager;
        public IndexTeacherModel(TestTest.Models.Db.DatabaseContext context, ILogger<IndexTeacherModel> logger, UserManager<Osoba> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public IList<Osoba> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_userManager.Users != null)
            {
                User = await _userManager.Users.ToListAsync();
            }
        }
    }
}
