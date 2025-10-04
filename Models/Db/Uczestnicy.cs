using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class Uczestnicy
    {
        [Display(Name = "ID uczestnika")]
        public int IdUczestnicy { get; set; }
        [Display(Name = "ID grupy")]
        public int? IdGrupy { get; set; }
        [Display(Name = "ID ucznia")]
        public int? IdUcznia { get; set; }

        public virtual Grupy? IdGrupyNavigation { get; set; }
        
    }
}
