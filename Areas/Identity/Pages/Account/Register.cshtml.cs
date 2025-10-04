#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestTest.Models.Db;

namespace TestTest.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Osoba> _signInManager;
        private readonly UserManager<Osoba> _userManager;
        private readonly IUserStore<Osoba> _userStore;
        private readonly IUserEmailStore<Osoba> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly TestTest.Models.Db.IdentityDatabaseContext _context;

        public RegisterModel(
            UserManager<Osoba> userManager,
            IUserStore<Osoba> userStore,
            SignInManager<Osoba> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            TestTest.Models.Db.IdentityDatabaseContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "To pole jest wymagane.")]
            [EmailAddress(ErrorMessage = "Adres e-mail jest niepoprawny.")]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Adres e-mail")]
            public string Email { get; set; }

            [Required(ErrorMessage = "To pole jest wymagane.")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+]).{8,}$",
                ErrorMessage = "Hasło musi mieć co najmniej 8 znaków, jedną małą literę, jedną dużą literę, jedną cyfrę i jeden znak specjalny.")]
            [StringLength(45, ErrorMessage = "Maksymalna długość to 45 znaków.")]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [StringLength(45, ErrorMessage = "Maksymalna długość to 45 znaków.")]
            [Compare("Password", ErrorMessage = "Wprowadzone hasła nie są takie same.")]
            public string ConfirmPassword { get; set; }

            [DataType(DataType.Text)]
            [MinLength(2, ErrorMessage = "Imię musi mieć co najmniej 2 znaki.")]
            [StringLength(45, ErrorMessage = "Maksymalna długość imienia to 45 znaków.")]
            [Display(Name = "Imię")]
            [Required(ErrorMessage = "To pole jest wymagane.")]
            public string Name { get; set; }

            [DataType(DataType.Text)]
            [MinLength(2, ErrorMessage = "Nazwisko musi mieć co najmniej 2 znaki.")]
            [StringLength(45, ErrorMessage = "Maksymalna długość nazwiska to 45 znaków.")]
            [Display(Name = "Nazwisko")]
            [Required(ErrorMessage = "To pole jest wymagane.")]
            public string Surname { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                var users = await _userManager.Users
                    .OrderByDescending(u => u.IdOsoba)
                    .Select(u => u.IdOsoba)
                    .ToListAsync();

                if(users == null)
                {
                    user.IdOsoba = 1;
                }
                else
                {
                    user.IdOsoba = users.FirstOrDefault()+1;
                }
                user.Name = Input.Name;
                user.Surname = Input.Surname;


                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Uczen");
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private Osoba CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Osoba>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Osoba)}'. " +
                    $"Ensure that '{nameof(Osoba)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Osoba> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Osoba>)_userStore;
        }
    }
}
