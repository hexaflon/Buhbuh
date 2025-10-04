using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class Status
    {
        public Status()
        {
            Osoba = new HashSet<Osoba>();
        }

        [Display(Name = "ID statusu")]
        public int IdStatus { get; set; }
        [Display(Name = "Nazwa statusu")]
        public string Nazwa { get; set; } = null!;
        [Display(Name = "Opis statusu")]
        public string Opis { get; set; } = null!;

        public virtual ICollection<Osoba> Osoba { get; set; }
    }
}
