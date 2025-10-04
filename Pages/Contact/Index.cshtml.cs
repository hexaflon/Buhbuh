using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Pages.Contact
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public ContactForm Form { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Tutaj mo¿na dodaæ logikê obs³ugi formularza, np. wysy³kê e-maila
            TempData["SuccessMessage"] = "Twoja wiadomoœæ zosta³a wys³ana!";
            return RedirectToPage();
        }

        public class ContactForm
        {
            [Required(ErrorMessage = "Imiê i nazwisko jest wymagane")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Adres email jest wymagany")]
            [EmailAddress(ErrorMessage = "Podaj poprawny adres email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Wiadomoœæ jest wymagana")]
            public string Message { get; set; }
        }
    }
}
