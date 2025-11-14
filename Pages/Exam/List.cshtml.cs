using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Exam
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

        public IList<Test> Test { get; set; } = default!;
        public IList<TestTest.Models.Db.Group> Groups { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string Visibility { get; set; }

        [BindProperty(SupportsGet = true)]
        public string GroupName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Test
                .OrderByDescending(t => t.Id)
                .Include(t => t.Groups)
                .Include(t => t.Questions)
                .ThenInclude(lp => lp.Questions)
                .AsQueryable();

            //do wyszukiwania testów
            if (!string.IsNullOrEmpty(Visibility))
            {
                var visibilityValue = bool.Parse(Visibility);
                query = query.Where(t => t.IsVisible == visibilityValue);
            }

            if (!string.IsNullOrEmpty(GroupName))
            {
                query = query.Where(t => t.Groups.Name == GroupName);
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                query = query
                    .Where(t => t.Questions.Any(lp => EF.Functions.Like(lp.Questions.Text, $"%{SearchText}%")));
            }

            if (User.IsInRole("Admin"))
            {
                Test = await _context.Test
                    .OrderByDescending(t => t.Id)
                    .Include(t => t.Groups)
                    .Include(t => t.Questions)
                    .ThenInclude(lp => lp.Questions).ToListAsync();
            }
            else
            {
                var iduser = _userManager.GetUserAsync(User).Result.PersonId;
                Test = await _context.Test
                    .OrderByDescending(t => t.Id)
                    .Where(t => t.TeacherId == iduser)
                    .Include(t => t.Groups)
                    .Include(t => t.Questions)
                    .ThenInclude(lp => lp.Questions).ToListAsync();
            }

            Groups = await _context.Group.ToListAsync();
        }
    }
}
