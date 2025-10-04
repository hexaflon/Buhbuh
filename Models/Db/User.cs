using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db;

public partial class User
{
    [Display(Name = "ID użytkownika")]
    public int Id { get; set; }

    [Display(Name = "Nazwa użytkownika")]
    [Required(ErrorMessage = "To pole jest wymagane.")]
    [StringLength(45, ErrorMessage = "Maksymalna długość to 45 znaków.")]
    public string Username { get; set; } = null!;

    [Display(Name = "Hasło")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+]).{8,}$", 
        ErrorMessage = "Hasło musi mieć co najmniej 8 znaków, jedną małą literę, jedną dużą literę, jedną cyfrę i jeden znak specjalny.")]
    [StringLength(45, ErrorMessage = "Maksymalna długość to 45 znaków.")]
    public string Password { get; set; } = null!;

    [Display(Name = "Imię i nazwisko")]
    [Required(ErrorMessage = "To pole jest wymagane.")]
    [StringLength(45, ErrorMessage = "Maksymalna długość to 45 znaków.")]
    public string? Name { get; set; }
}
