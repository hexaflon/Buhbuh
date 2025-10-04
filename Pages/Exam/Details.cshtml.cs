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
        private readonly UserManager<Osoba> _userManager;
        public DetailsModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Test Test { get; set; } = default!;
        public List<Wynik> Wyniki { get; set; } = default!;
        public List<double> Oceny { get; set; } = new List<double>();
        public IList<Rozwiazanie> Rozwiazanie { get; set; } = default!;
        public double Avg { get; set; } = 0.0;


        public void Srednia()
        {
            double suma = 0;
            int liczbaPytan = Test.ListaPytan.Count();
            foreach(var wynik in Wyniki)
            {
                var division = wynik.punkty / liczbaPytan;
                division *= 100;
                division %= 100;
                if (division >= 90) suma += 5;
                else if (division >= 80 && division < 90) suma += 4.5;
                else if (division >= 70 && division < 80) suma += 4;
                else if (division >= 60 && division < 70) suma += 3.5;
                else if (division >= 50 && division < 60) suma += 3;
                else suma += 2;
            }
            Avg = suma / Wyniki.Count();
        }

        public void Ocena()
        {
            if (Oceny.Count == 0)
            {
                foreach (var wynik in Rozwiazanie)
                {
                    var division = wynik.LiczbaPunktow / wynik.IdTestNavigation.ListaPytan.Count();
                    division *= 100;
                    division %= 100;
                    if (division >= 90) Oceny.Add(5);
                    else if (division >= 80 && division < 90) Oceny.Add(4.5);
                    else if (division >= 70 && division < 80) Oceny.Add(4);
                    else if (division >= 60 && division < 70) Oceny.Add(3.5);
                    else if (division >= 50 && division < 60) Oceny.Add(3);
                    else Oceny.Add(2);
                }
            }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var test = await _context.Test.Include(t=>t.ListaPytan).FirstOrDefaultAsync(m => m.IdTest == id);
            if (test == null)
            {
                return NotFound();
            }
            
            var rozwiazania = _context.Rozwiazanie.Where(r => r.IdTest == id).ToList();
            Wyniki = new List<Wynik>();
            Rozwiazanie = rozwiazania;
            foreach (var rozwiazanie in rozwiazania)
            {
                var user = _userManager.Users
                    .Where(u => u.IdOsoba == rozwiazanie.IdUcznia)
                    .Select(u => new {u.Name, u.Surname}).First();
                var wynik = new Wynik();
                wynik.idUcznia = rozwiazanie.IdUcznia;
                wynik.imie = user.Name;
                wynik.nazwisko = user.Surname;
                wynik.punkty = (double)rozwiazanie.LiczbaPunktow;

                wynik.nowaWiadomosc(test.ListaPytan.Count());
                Wyniki.Add(wynik);

            }

            Test = test;
            Srednia();
            Ocena();
            return Page();
        }
    }
}
