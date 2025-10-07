using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Pages.Question;
using TestTest.Models.Db;

namespace TestTest.Pages.Question
{
    [Authorize(Roles = "Nauczyciel,Admin")]
    public class IndexModel : PageModel
    {
        private readonly TestTest.Models.Db.DatabaseContext _context;
        private readonly UserManager<Osoba> _userManager;
        public IndexModel(TestTest.Models.Db.DatabaseContext context, UserManager<Osoba> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Pytanie> Pytanie { get; set; } = default!;
        public IList<KategoriaPytania> Kategorie { get; set; } = default!;
        public IList<TypPytania> TypyPytan { get; set; } = default!;

        public int? CategoryId { get; set; }
        public int? TypeId { get; set; }
        public string SearchText { get; set; }

        public async Task OnGetAsync(int? categoryId, int? typeId, string searchText)
        {
            var facade = new QuestionFacade(_context);

            Pytanie = facade.GetPytania(
                categoryId: categoryId,
                typeId: typeId,
                searchText: searchText
                );

            Kategorie = await _context.KategoriaPytania.ToListAsync();
            TypyPytan = await _context.TypPytania.ToListAsync();
        }



        public IActionResult OnGetAddAnswer(int idPytanie)
        {
            if (idPytanie == null) return Page();

            return RedirectToPage("/Answer/Create", new { id = idPytanie });
        }
    }
}
