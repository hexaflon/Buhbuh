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

namespace ProjektInzynierski.Pages.Exam
{
    [Authorize(Roles = "Uczen,Admin")]
    public class ExamListModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;
        public ExamListModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Test> Test { get;set; } = default!;
        public IList<bool> isSolved { get; set; } = new List<bool>();

        public void checkIsSolved()
        {
            foreach(var t in Test)
            {
                var studentId = _userManager.GetUserAsync(User).Result.PersonId;
                if (_context.Result
                    .Where(r => r.TestId == t.Id && r.StudentId == studentId).FirstOrDefault() == null) isSolved.Add(false);
                else isSolved.Add(true);

            }
        }

        public async Task OnGetAsync()
        {
            if (_context.Test != null)
            {

                if (User.IsInRole("Admin"))
                {
                    Test = _context.Test
                        .Where(t => t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now)
                        .ToList();
                }
                else { 
                    var studentId = _userManager.GetUserAsync(User).Result.PersonId;
                    var studentGroup = _context.Participant.Where(u => u.StudentId == studentId)
                        .Select(u => u.GroupId);
                    Test = _context.Test
                    .Where(t => studentGroup.Contains(t.GroupId))
                    .Where(t => t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now).ToList();
                    checkIsSolved();                
                }
            }
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] int? idTest)
        {
            if (idTest == null) return NotFound();
            var test = _context.Test.FindAsync(idTest);
            if (test == null) return NotFound();

            Result result = new Result();
            result.StudentId = _userManager.GetUserAsync(User).Result.PersonId;
            result.TestId = idTest;
            var resultList = _context.Result.ToList();
            if (resultList == null) result.Id = 0;
            else
            {
                result.Id = resultList
                    .OrderByDescending(r => r.Id)
                    .Select(r => r.Id).FirstOrDefault();
            }

            _context.Result.Add(result);
            await _context.SaveChangesAsync();

            
            return RedirectToPage("ExamPerform",new {idrozwiazania = result.Id});
        
        }
    }
}