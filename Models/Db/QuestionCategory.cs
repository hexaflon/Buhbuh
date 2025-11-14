using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class QuestionCategory
    {
        public QuestionCategory()
        {
            Question = new HashSet<Question>();
        }

        [Display(Name = "ID kategorii pytania")]
        public int Id { get; set; }
        [Display(Name = "Kategoria pytania")]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [StringLength(45, ErrorMessage = "Maksymalna długość to 45 znaków.")]
        public string Name { get; set; } = null!;
        [Display(Name = "Opis kategorii")]
        [StringLength(255, ErrorMessage = "Maksymalna długość to 255 znaków.")]
        public string Description { get; set; } = null!;

        public virtual ICollection<Question> Question { get; set; }
    }
}
