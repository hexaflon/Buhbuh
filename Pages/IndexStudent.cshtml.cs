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
    [Authorize(Roles = "Uczen,Admin")]
    public class IndexStudentModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;

        public IndexStudentModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Test> Test { get; set; } = default!;
        public IList<Result> Result { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Test != null)
            {
                if (User.IsInRole("Admin"))
                {
                    Test = _context.Test
                        .Where(t => t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now)
                        .OrderBy(t => t.EndDate)
                        .ToList();
                }
                else
                {
                    var studentId = _userManager.GetUserAsync(User).Result.PersonId;
                    var studentGroup = _context.Participant.Where(u => u.StudentId == studentId)
                        .Select(u => u.GroupId);

                    Test = _context.Test
                        .Where(t => studentGroup.Contains(t.GroupId))
                        .Where(t => t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now)
                        .OrderBy(t => t.EndDate)
                        .ToList();
                }
            }
        }
    }
}
