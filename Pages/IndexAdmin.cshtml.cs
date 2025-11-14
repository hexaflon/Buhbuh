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
    [Authorize(Roles ="Admin")]
    public class IndexAdminModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;
        public IndexAdminModel(TestTest.Models.Db.DatabaseContext context, ILogger<IndexAdminModel> logger, UserManager<Person> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<Person> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_userManager.Users != null)
            {
                User = await _userManager.Users.ToListAsync();
            }
        }
    }
}
