using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class Rozwiazanie
    {
        public Rozwiazanie()
        {
            RozwiazanieDoPytan = new HashSet<RozwiazanieDoPytan>();
        }

        [Display(Name = "Rozwiązanie")]
        public int IdRozwiazanie { get; set; }
        public int? IdUcznia { get; set; }
        public int? IdTest { get; set; }
        [Display(Name = "Liczba punktów")]
        public double? LiczbaPunktow { get; set; }

        [Display(Name = "Tytuł testu")]
        public virtual Test? IdTestNavigation { get; set; }
        
        public virtual ICollection<RozwiazanieDoPytan> RozwiazanieDoPytan { get; set; }
    }
}
