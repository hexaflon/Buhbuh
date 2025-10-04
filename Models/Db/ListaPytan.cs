using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class ListaPytan
    {
        [Display(Name = "ID listy pytań")]
        public int IdListaPytan { get; set; }
        public int? IdTest { get; set; }
        public int? IdPytanie { get; set; }

        public virtual Pytanie? IdPytanieNavigation { get; set; }
        public virtual Test? IdTestNavigation { get; set; }
    }
}
