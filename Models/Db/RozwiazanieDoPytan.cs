using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class RozwiazanieDoPytan
    {
        public int IdRozwiazanieDoPytan { get; set; }
        public int? IdOdpowiedz { get; set; }
        public int? IdRozwiazanie { get; set; }

        public virtual Odpowiedz? IdOdpowiedzNavigation { get; set; }
        public virtual Rozwiazanie? IdRozwiazanieNavigation { get; set; }
    }
}
