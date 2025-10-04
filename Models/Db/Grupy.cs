using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class Grupy
    {
        public Grupy()
        {
            Test = new HashSet<Test>();
            Uczestnicy = new HashSet<Uczestnicy>();
        }

        [Display(Name = "ID grupy")]
        public int IdGrupy { get; set; }
        [Display(Name = "ID nauczyciela")]
        public int? IdNauczyciela { get; set; }
        [Display(Name = "Nazwa grupy")]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [StringLength(45, ErrorMessage = "Maksymalna długość to 45 znaków.")]
        public string Nazwa { get; set; } = null!;

        
        public virtual ICollection<Test> Test { get; set; }
        public virtual ICollection<Uczestnicy> Uczestnicy { get; set; }
    }
}
