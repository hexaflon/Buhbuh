using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class TypPytania
    {
        public TypPytania()
        {
            Pytanie = new HashSet<Pytanie>();
        }

        [Display(Name = "ID typu pytania")]
        public int IdTypPytania { get; set; }
        [Display(Name = "Typ pytania")]
        [StringLength(45, ErrorMessage = "Maksymalna długość to 45 znaków.")]
        public string Nazwa { get; set; } = null!;
        [Display(Name = "Opis pytania")]
        [StringLength(255, ErrorMessage = "Maksymalna długość to 255 znaków.")]
        public string Opis { get; set; } = null!;

        public virtual ICollection<Pytanie> Pytanie { get; set; }
    }
}
