using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;


namespace TestTest.Models.Db
{
    public partial class Osoba : IdentityUser
    {
        [Display(Name = "ID użytkownika")]
        public int IdOsoba { get; set; }
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [MinLength(2, ErrorMessage = "Imię musi mieć co najmniej 2 znaki.")]
        [StringLength(45, ErrorMessage = "Maksymalna długość to 45 znaków.")]
        public string Name { get; set; } = null!;
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [MinLength(2, ErrorMessage = "Nazwisko musi mieć co najmniej 2 znaki.")]
        [StringLength(45, ErrorMessage = "Maksymalna długość to 45 znaków.")]
        public string Surname { get; set; } = null!;

    }
}
