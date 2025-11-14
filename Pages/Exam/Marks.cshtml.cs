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

namespace ProjektInzynierski.Pages.Exam
{
    [Authorize(Roles = "Uczen,Admin")]
    public class MarksModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;

        public MarksModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Result> Results { get;set; } = default!;
        public List<double> Marks { get; set; } = new List<double>();

        public void Mark()
        {
            foreach (var result in Results)
            {
                var division = result.Points / result.Tests.Questions.Count();
                division *= 100;
                division %= 100;
                if (division >= 90) Marks.Add(5);
                else if (division >= 80 && division < 90) Marks.Add(4.5);
                else if (division >= 70 && division < 80) Marks.Add(4);
                else if (division >= 60 && division < 70) Marks.Add(3.5);
                else if (division >= 50 && division < 60) Marks.Add(3);
                else Marks.Add(2);
            }
        }

        public async Task OnGetAsync()
        {
            if (_context.Result != null)
            {
                if (User.IsInRole("Admin"))
                {
                    Results = await _context.Result
                    .Include(r => r.Tests)
                    .ThenInclude(t=>t.Questions).ToListAsync();
                }
                else
                {
                    var id = _userManager.GetUserAsync(User).Result.PersonId;
                    Results = await _context.Result
                    .Include(r => r.Tests)
                    .ThenInclude(t => t.Questions)
                    .Where(r => r.StudentId == id)
                    .ToListAsync();
                }
                Mark();
            }
        }
    }
}
