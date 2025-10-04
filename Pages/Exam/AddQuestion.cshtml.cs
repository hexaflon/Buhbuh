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

namespace TestTest.Pages.Exam
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class AddQuestionModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;

        public AddQuestionModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<KategoriaPytania> Kategorie { get; set; } = default!;
        public IList<TypPytania> TypyPytan { get; set; } = default!;

        public int? CategoryId { get; set; }
        public int? TypeId { get; set; }
        public string SearchText { get; set; }

        public IList<Pytanie> lPytan { get; set; } = new List<Pytanie>();

        public IActionResult OnGet([FromQuery] int? id, int? categoryId, int? typeId, string searchText)
        {
            if (id.HasValue)
            {
                var testList = _context.Test.Include(t => t.ListaPytan).ThenInclude(lp => lp.IdPytanieNavigation).ToList();
                wysTest = (from test in testList
                           where test.IdTest == id
                           select test).FirstOrDefault();
                if (wysTest == null) return NotFound();
                ViewData["IdTest"] = id;
            }
            else
            {
                return RedirectToPage("/List");
            }
            var pytania =  _context.Pytanie.Include(p => p.Odpowiedz).Where(p => p.Odpowiedz.Count()!=0).ToList();
            var pytaniaWTescie = _context.ListaPytan.Where(lp => lp.IdTest == id).Select(lp => lp.IdPytanie).ToList();
            pytania = pytania.Where(p => !pytaniaWTescie.Contains(p.IdPytanie)).ToList();

            if (categoryId.HasValue) pytania = pytania.Where(p => p.IdKategoriaPytania == categoryId).ToList();
            if (typeId.HasValue) pytania = pytania.Where(p => p.IdTypPytania == typeId).ToList();
            if (!string.IsNullOrEmpty(searchText)) pytania = pytania.Where(p => p.Tresc.Contains(searchText)).ToList();
          

            ViewData["IdPytanie"] = new SelectList(pytania, "IdPytanie", "Tresc");
            lPytan = pytania;
            Kategorie =  _context.KategoriaPytania.ToList();
            TypyPytan =  _context.TypPytania.ToList();
            return Page();
        }

        [BindProperty]
        public ListaPytan ListaPytan { get; set; } = default!;

        public Test wysTest { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync([FromQuery] int? id)
        {
            if (!id.HasValue) return NotFound();
            if (!ModelState.IsValid || _context.ListaPytan == null || ListaPytan == null)
            {
                return Page();
            }
            if (_context.ListaPytan == null) ListaPytan.IdListaPytan = 0;
            else
            {
                var lListaPytan = _context.ListaPytan.ToList();
                ListaPytan.IdListaPytan = (from lp in lListaPytan
                                         orderby lp.IdListaPytan descending
                                         select lp.IdListaPytan).FirstOrDefault() + 1;

            }
            if(ListaPytan.IdPytanie==null) return RedirectToPage("", new { id = id });
            ListaPytan.IdTest = id;
            _context.ListaPytan.Add(ListaPytan);
            await _context.SaveChangesAsync();

            return RedirectToPage("",new { id = id });
        }
        public async Task<IActionResult> OnGetDelete([FromQuery] int? idTest, [FromQuery] int? idPytanie)
        {

            if (idTest == null || idPytanie == null) return NotFound();
            var pytanieDoUsuniecia = await _context.ListaPytan.FirstOrDefaultAsync(lp => lp.IdTest == idTest && lp.IdPytanie == idPytanie);

            if (pytanieDoUsuniecia != null)
            {
                _context.ListaPytan.Remove(pytanieDoUsuniecia);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("", new { id = idTest });
        }
    }
}
