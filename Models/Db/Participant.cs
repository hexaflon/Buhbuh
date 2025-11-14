using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class Participant
    {
        [Display(Name = "ID uczestnika")]
        public int Id { get; set; }
        [Display(Name = "ID grupy")]
        public int? GroupId { get; set; }
        [Display(Name = "ID ucznia")]
        public int? StudentId { get; set; }

        public virtual Group? Groups { get; set; }
        
    }
}
