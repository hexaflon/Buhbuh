using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProjektInzynierski.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        
        public RedirectToPageResult OnGet()
        {
            if (User.IsInRole("Uczen")) return RedirectToPage("IndexStudent");
            if (User.IsInRole("Nauczyciel")) return RedirectToPage("IndexTeacher");
            if (User.IsInRole("Admin")) return RedirectToPage("IndexAdmin");
            return RedirectToPage("");
        }
    }
}
