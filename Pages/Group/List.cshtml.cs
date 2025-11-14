using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class ListModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;
        public ListModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<TestTest.Models.Db.Group> Group { get;set; } = default!;

        public async Task OnGetAsync(string? searchText)
        {
            IQueryable<TestTest.Models.Db.Group> query = _context.Group;

            if (User.IsInRole("Admin"))
            {
                // administrator widzi wszystkie grupy
            }
            else
            {
                var userId = _userManager.GetUserAsync(User).Result.PersonId;
                query = query.Where(g => g.TeacherId == userId);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(g => g.Name.Contains(searchText));
            }

            Group = await query.OrderByDescending(g=>g.Id).ToListAsync();
        }

    }
}
