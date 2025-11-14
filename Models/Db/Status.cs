using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class Status
    {
        public Status()
        {
            Person = new HashSet<Person>();
        }

        [Display(Name = "ID statusu")]
        public int Id { get; set; }
        [Display(Name = "Nazwa statusu")]
        public string Name { get; set; } = null!;
        [Display(Name = "Opis statusu")]
        public string Description { get; set; } = null!;

        public virtual ICollection<Person> Person { get; set; }
    }
}
