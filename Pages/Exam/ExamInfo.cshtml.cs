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
    [Authorize]
    public class ExamInfoModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;

        public ExamInfoModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager, SignInManager<Person> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Result Result { get; set; } = default!;
        public Test Test { get; set; } = default!;
        public List<double> Marks { get; set; } = new List<double>();
        public int OriginalTestId { get; set; } //do powrotu na stronę Details

        public void Mark()
        {
            var division = Result.Points / Result.Tests.Questions.Count();
            division *= 100;
            division %= 100;
            if (division >= 90) Marks.Add(5);
            else if (division >= 80 && division < 90) Marks.Add(4.5);
            else if (division >= 70 && division < 80) Marks.Add(4);
            else if (division >= 60 && division < 70) Marks.Add(3.5);
            else if (division >= 50 && division < 60) Marks.Add(3);
            else Marks.Add(2);
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] int id)
        {

            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var result = await _context.Result.FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var userId = _userManager.GetUserAsync(User).Result.PersonId;
                if (userId == result.StudentId || User.IsInRole("Admin") || User.IsInRole("Nauczyciel"))
                {
                    Test = await _context.Test
                        .Include(t => t.Questions)
                        .FirstOrDefaultAsync(t => t.Id == result.TestId);
                    if (Test == null) return NotFound();
                    OriginalTestId = Test.Id;
                    Result = result;
                }
                else return Forbid();
                Mark();
            }
            return Page();
        }
    }
}
