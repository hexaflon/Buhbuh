using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class Group
    {
        public Group()
        {
            Test = new HashSet<Test>();
            Participants = new HashSet<Participant>();
        }

        [Display(Name = "ID grupy")]
        public int Id { get; set; }

        [Display(Name = "ID nauczyciela")]
        public int? TeacherId { get; set; }
        [Display(Name = "Nazwa grupy")]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [StringLength(45, ErrorMessage = "Maksymalna długość to 45 znaków.")]
        public string Name { get; set; } = null!;

        
        public virtual ICollection<Test> Test { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }

        public override string ToString()
        {
            return $"Grupa o id: {Id} - {Name}";
        }
    }
}
