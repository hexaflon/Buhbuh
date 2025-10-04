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
using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class AddMembersModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;

        public AddMembersModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet([FromQuery]int? id)
        {
            if (!id.HasValue) return RedirectToPage("./List");
            

            var uzytkownicy = _userManager.GetUsersInRoleAsync("Uczen");
            var uczniowie = uzytkownicy.Result.ToList();
            uczniowie = uczniowie.Where(o => !_context.Uczestnicy.Any(u => u.IdUcznia == o.IdOsoba && u.IdGrupy == id)).ToList();


            var wysuzytkownicy = _userManager.GetUsersInRoleAsync("Uczen");
            wysUczniowie = wysuzytkownicy.Result.ToList();
            wysUczniowie = wysUczniowie
                .Where(o => _context.Uczestnicy.Any(u => u.IdUcznia == o.IdOsoba && u.IdGrupy == id))
                .ToList();

            var uczniowieList = uczniowie.Select(osoba => new SelectListItem
            {
                Value = osoba.IdOsoba.ToString(),
                Text = $"{osoba.Surname} {osoba.Name} {osoba.Email}"
            }).ToList();
            ViewData["IdUcznia"] = new SelectList(uczniowieList, "Value","Text");
            ViewData["idGrupy"] = id; 
            return Page();
        }

        [BindProperty]
        public Uczestnicy Uczestnicy { get; set; } = default!;
        public List<Osoba> wysUczniowie { get; set; } = default!;


        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync([FromQuery]int? id)
        {
            if (!id.HasValue) return RedirectToPage("./List");

            if (!ModelState.IsValid || _context.Uczestnicy == null || Uczestnicy == null)
            {
                return Page();
            }

            int? grupyList = _context.Grupy.Where(gr => gr.IdGrupy == id).Select(gr => gr.IdGrupy).FirstOrDefault();
            if (grupyList == null) return NotFound();
            Uczestnicy.IdGrupy = id;

            var uczestnicyList = _context.Uczestnicy.ToList();
            if (uczestnicyList == null) Uczestnicy.IdUczestnicy = 0;
            else
            {
                Uczestnicy.IdUczestnicy = uczestnicyList.OrderByDescending(u => u.IdUczestnicy)
                                                        .Select(u => u.IdUczestnicy).FirstOrDefault() + 1;
            }

            
            //dodać dla użytkownika
            

            _context.Uczestnicy.Add(Uczestnicy);
            await _context.SaveChangesAsync();

            return RedirectToPage("", new {id=id});
        }


        

        public async Task<IActionResult> OnGetDelete([FromQuery]int? idUcznia, [FromQuery]int? idGrupy)
        {
            
            if(idUcznia == null || idGrupy == null) return NotFound();
            var uczestnikDoUsunieca = await _context.Uczestnicy.FirstOrDefaultAsync(u => u.IdUcznia == idUcznia && u.IdGrupy == idGrupy);
            
            if (uczestnikDoUsunieca != null)
            {
                _context.Uczestnicy.Remove(uczestnikDoUsunieca);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("", new { id = idGrupy });
        }
    }
}
