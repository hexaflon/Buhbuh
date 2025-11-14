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
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class DetailsModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;
        public DetailsModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Test Test { get; set; } = default!;
        public List<Score> Scores { get; set; } = default!;
        public List<double> Mark { get; set; } = new List<double>();
        public IList<Result> Results { get; set; } = default!;
        public double Avg { get; set; } = 0.0;


        public void Average()
        {
            double sum = 0;
            int questionCount = Test.Questions.Count();
            foreach(var score in Scores)
            {
                var division = score.points / questionCount;
                division *= 100;
                division %= 100;
                if (division >= 90) sum += 5;
                else if (division >= 80 && division < 90) sum += 4.5;
                else if (division >= 70 && division < 80) sum += 4;
                else if (division >= 60 && division < 70) sum += 3.5;
                else if (division >= 50 && division < 60) sum += 3;
                else sum += 2;
            }
            Avg = sum / Scores.Count();
        }

        public void MarkTest()
        {
            if (Mark.Count == 0)
            {
                foreach (var score in Results)
                {
                    var division = score.Points / score.Tests.Questions.Count();
                    division *= 100;
                    division %= 100;
                    if (division >= 90) Mark.Add(5);
                    else if (division >= 80 && division < 90) Mark.Add(4.5);
                    else if (division >= 70 && division < 80) Mark.Add(4);
                    else if (division >= 60 && division < 70) Mark.Add(3.5);
                    else if (division >= 50 && division < 60) Mark.Add(3);
                    else Mark.Add(2);
                }
            }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var test = await _context.Test.Include(t=>t.Questions).FirstOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }
            
            var scores = _context.Result.Where(r => r.TestId == id).ToList();
            Scores = new List<Score>();
            Results = scores;
            foreach (var result in scores)
            {
                var user = _userManager.Users
                    .Where(u => u.PersonId == result.StudentId)
                    .Select(u => new {u.Name, u.Surname}).First();
                var wynik = new Score();
                wynik.studentId = result.StudentId;
                wynik.name = user.Name;
                wynik.surname = user.Surname;
                wynik.points = (double)result.Points;

                wynik.newMessage(test.Questions.Count());
                Scores.Add(wynik);

            }

            Test = test;
            Average();
            MarkTest();
            return Page();
        }
    }
}
