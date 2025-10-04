using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db;

public partial class Pytanie
{

        public Pytanie()
        {
            ListaPytan = new HashSet<ListaPytan>();
            Odpowiedz = new HashSet<Odpowiedz>();
        }

        [Display(Name = "ID pytania")]
        public int IdPytanie { get; set; }
        [Display(Name = "ID nauczyciela")]
        public int? IdNauczyciela { get; set; }
        [Display(Name = "Kategoria pytania")]
        public int? IdKategoriaPytania { get; set; }
        [Display(Name = "Typ pytania")]
        public int? IdTypPytania { get; set; }
        [Display(Name = "Treść pytania")]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [StringLength(255, ErrorMessage = "Maksymalna długość to 255 znaków.")]
        [MinLength(5, ErrorMessage = "Pytanie musi mieć co najmnniej 5 znaków.")]
        public string? Tresc { get; set; }

        [Display(Name = "Kategoria pytania")]
        public virtual KategoriaPytania? IdKategoriaPytaniaNavigation { get; set; }
        
        [Display(Name = "ID typu pytania")]
        public virtual TypPytania? IdTypPytaniaNavigation { get; set; }
        public virtual ICollection<ListaPytan> ListaPytan { get; set; }
        public virtual ICollection<Odpowiedz> Odpowiedz { get; set; }
    
}
