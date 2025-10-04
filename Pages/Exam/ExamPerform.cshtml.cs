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
        private readonly UserManager<Osoba> _userManager;
        private static int idWielokrotngo;

        public ExamPerformModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
            idWielokrotngo =  _context.TypPytania
                .Where(tp => tp.Nazwa == "Wielokrotnego wyboru")
                .Select(tp => tp.IdTypPytania).FirstOrDefault();
        }

        public List<Pytanie> pytaniaSprawdzianu { get;set; } = default!;
        public int CzasTrwania { get; set; }

        public List<Pytanie> GetQuestions(int examId)
        {
            var questions = _context.ListaPytan
                .Include(lp => lp.IdPytanieNavigation)
                .ThenInclude(p => p.Odpowiedz)
                .Where(lp => lp.IdTest == examId)
                .Select(lp => lp.IdPytanieNavigation).ToList();

            return questions;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            pytaniaSprawdzianu = GetQuestions(id);
            if (pytaniaSprawdzianu == null) return NotFound();
            
            ViewData["idWielokrotnego"] = idWielokrotngo;

            var test = await _context.Test.FindAsync(id);
            CzasTrwania = test.CzasTrwania ?? 0;

            return Page();
        }
        //, Dictionary<int, int[]> selectedAnswers

        public async void RozwiazanieDoPytanPrzetworzenie(int id, int idOdpowiedz, int idRozwiazanie)
        {
            int rDPId;
            Console.WriteLine($"id:{id} Odpowiedz:{idOdpowiedz} rozwiazanie:{idRozwiazanie}");
            if (_context.RozwiazanieDoPytan.ToList() == null) rDPId = 1;
            else
            {
                rDPId = _context.RozwiazanieDoPytan.OrderByDescending(rdp => rdp.IdRozwiazanieDoPytan)
                    .Select(rdp => rdp.IdRozwiazanieDoPytan).FirstOrDefault() + 1;
            }
            var RDP = new RozwiazanieDoPytan();
            RDP.IdRozwiazanieDoPytan = rDPId;
            RDP.IdOdpowiedz = idOdpowiedz;
            RDP.IdRozwiazanie = idRozwiazanie;
            _context.RozwiazanieDoPytan.Add(RDP);
            rDPId++;
            _context.SaveChanges();
        }


        public async Task<IActionResult> OnPostAsync(int id)
        {
            var selectedAnswers = Request.Form;
            int ilePoprawnych = 0, zaznaczonePoprawne=0;
            double punkty = 0;
            Console.WriteLine(selectedAnswers);
            
            Rozwiazanie rozwiazanieSprawdzianu = new Rozwiazanie();
            if (_context.Rozwiazanie.ToList() == null) rozwiazanieSprawdzianu.IdRozwiazanie = 1;
            else
            {
                rozwiazanieSprawdzianu.IdRozwiazanie =
                    _context.Rozwiazanie.OrderByDescending(r => r.IdRozwiazanie)
                    .Select(r => r.IdRozwiazanie).FirstOrDefault() + 1;
            }

            List<int> odpIds = new List<int>();

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

                var pytanie = _context.Pytanie.Include(p => p.Odpowiedz)
                    .Where(p => p.IdPytanie == key).FirstOrDefault();
                bool czyWielokrotnego = pytanie.IdTypPytania == idWielokrotngo;
                if (czyWielokrotnego)
                {
                    ilePoprawnych = pytanie.Odpowiedz
                        .Where(o => o.CzyPoprawny == true)
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
                    var isCorrect = pytanie.Odpowiedz.Any(o => o.IdOdpowiedz == val && o.CzyPoprawny);


                    odpIds.Add(val);


                    if (czyWielokrotnego)
                    {
                        if (isCorrect)
                        {
                            zaznaczonePoprawne++;
                        }
                        else
                        {
                            zaznaczonePoprawne--;
                        }
                    }
                    else
                    {
                        if (isCorrect)
                        {
                            punkty++;
                        }
                    }
                }
                if (czyWielokrotnego)
                {
                    double punkt = (double)zaznaczonePoprawne / (double)ilePoprawnych;
                    if (zaznaczonePoprawne > 0) {
                        
                        punkty += punkt; }
                    Console.WriteLine($"Po {punkty} {zaznaczonePoprawne} {ilePoprawnych}, {punkt}");
                    zaznaczonePoprawne = 0;
                    ilePoprawnych = 0;
                }
            }
            

            

            rozwiazanieSprawdzianu.IdTest = id;
            rozwiazanieSprawdzianu.IdUcznia = _userManager.GetUserAsync(User).Result.IdOsoba;
            rozwiazanieSprawdzianu.LiczbaPunktow = punkty;
            _context.Rozwiazanie.Add(rozwiazanieSprawdzianu);


            






            _context.SaveChanges();
            foreach(var odp in odpIds)
            {

                RozwiazanieDoPytanPrzetworzenie(id, odp, rozwiazanieSprawdzianu.IdRozwiazanie);
            }

            return RedirectToPage("./ExamInfo", new { id = rozwiazanieSprawdzianu.IdRozwiazanie });
        }
    }
}
