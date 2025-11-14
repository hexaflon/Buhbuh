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
    public class ExamPerformModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Person> _userManager;
        private static int idWielokrotngo;

        public ExamPerformModel(TestTest.Models.Db.DatabaseContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
            idWielokrotngo =  _context.QuestionType
                .Where(tp => tp.Name == "Wielokrotnego wyboru")
                .Select(tp => tp.Id).FirstOrDefault();
        }

        public List<TestTest.Models.Db.Question> testQuestions { get;set; } = default!;
        public int Duration { get; set; }

        public List<TestTest.Models.Db.Question> GetQuestions(int examId)
        {
            var questions = _context.QuestionList
                .Include(lp => lp.Questions)
                .ThenInclude(p => p.Answers)
                .Where(lp => lp.TestId == examId)
                .Select(lp => lp.Questions).ToList();

            return questions;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            testQuestions = GetQuestions(id);
            if (testQuestions == null) return NotFound();
            
            ViewData["idWielokrotnego"] = idWielokrotngo;

            var test = await _context.Test.FindAsync(id);
            Duration = test.Duration ?? 0;

            return Page();
        }
        //, Dictionary<int, int[]> selectedAnswers

        public async void RozwiazanieDoPytanPrzetworzenie(int id, int idOdpowiedz, int idRozwiazanie)
        {
            int rDPId;
            Console.WriteLine($"id:{id} Odpowiedz:{idOdpowiedz} rozwiazanie:{idRozwiazanie}");
            if (_context.QuestionResult.ToList() == null) rDPId = 1;
            else
            {
                rDPId = _context.QuestionResult.OrderByDescending(rdp => rdp.Id)
                    .Select(rdp => rdp.Id).FirstOrDefault() + 1;
            }
            var RDP = new QuestionResult();
            RDP.Id = rDPId;
            RDP.AnswerId = idOdpowiedz;
            RDP.ResultId = idRozwiazanie;
            _context.QuestionResult.Add(RDP);
            rDPId++;
            _context.SaveChanges();
        }


        public async Task<IActionResult> OnPostAsync(int id)
        {
            var selectedAnswers = Request.Form;
            int correctCount = 0, selectedCorrect=0;
            double points = 0;
            Console.WriteLine(selectedAnswers);
            
            Result testResult = new Result();
            if (_context.Result.ToList() == null) testResult.Id = 1;
            else
            {
                testResult.Id =
                    _context.Result.OrderByDescending(r => r.Id)
                    .Select(r => r.Id).FirstOrDefault() + 1;
            }

            List<int> answerList = new List<int>();

            foreach (var question in selectedAnswers)
            {
                var temp = question.Key;
                int key=0;
                Console.WriteLine($"Converting '{temp}' to integer...");
                try
                {
                    key = Convert.ToInt32(temp);
                    Console.WriteLine($"Successfully converted '{temp}' to {key}.");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Failed to convert '{temp}' to integer: {ex.Message}");
                    continue;
                }

                var testQuestion = _context.Question.Include(p => p.Answers)
                    .Where(p => p.Id == key).FirstOrDefault();
                bool czyWielokrotnego = testQuestion.TypeId == idWielokrotngo;
                if (czyWielokrotnego)
                {
                    correctCount = testQuestion.Answers
                        .Where(o => o.IsCorrect == true)
                        .Count();
                }


                


                foreach (var answer in question.Value)
                {
                    temp = answer;
                    int val = 0;
                    Console.WriteLine($"Converting '{temp}' to integer...");
                    
                    
                    try
                    {
                        val = Convert.ToInt32(temp);
                        Console.WriteLine($"Successfully converted '{temp}' to {val}.");
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Failed to convert '{temp}' to integer: {ex.Message}");
                        continue;
                    }
                    var isCorrect = testQuestion.Answers.Any(o => o.Id == val && o.IsCorrect);


                    answerList.Add(val);


                    if (czyWielokrotnego)
                    {
                        if (isCorrect)
                        {
                            selectedCorrect++;
                        }
                        else
                        {
                            selectedCorrect--;
                        }
                    }
                    else
                    {
                        if (isCorrect)
                        {
                            points++;
                        }
                    }
                }
                if (czyWielokrotnego)
                {
                    double punkt = (double)selectedCorrect / (double)correctCount;
                    if (selectedCorrect > 0) {
                        
                        points += punkt; }
                    Console.WriteLine($"Po {points} {selectedCorrect} {correctCount}, {punkt}");
                    selectedCorrect = 0;
                    correctCount = 0;
                }
            }
            

            

            testResult.TestId = id;
            testResult.StudentId = _userManager.GetUserAsync(User).Result.PersonId;
            testResult.Points = points;
            _context.Result.Add(testResult);


            






            _context.SaveChanges();
            foreach(var odp in answerList)
            {

                RozwiazanieDoPytanPrzetworzenie(id, odp, testResult.Id);
            }

            return RedirectToPage("./ExamInfo", new { id = testResult.Id });
        }
    }
}
