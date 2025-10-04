using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Exam
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class CreateModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        public CreateModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserAsync(User).Result.IdOsoba; 
            ViewData["IdGrupy"] = new SelectList(_context.Grupy.Where(g => g.IdNauczyciela==userId), "IdGrupy", "Nazwa");
            return Page();
        }

        [BindProperty]
        public Test Test { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Test == null || Test == null)
            {
                return Page();
            }
            Test.DataUtworzenia = DateTime.Now;
            //dodać dla obecnego użytkownika
            Test.IdNauczyciela = _userManager.GetUserAsync(User).Result.IdOsoba;

            var testyList = _context.Test.ToList();
            if (testyList == null) Test.IdTest = 0;
            else
            {
                Test.IdTest = testyList.OrderByDescending(test => test.IdTest).Select(test => test.IdTest).FirstOrDefault()+1;
            }

            _context.Test.Add(Test);
            await _context.SaveChangesAsync();

            return RedirectToPage("./List");
        }
    }
}
