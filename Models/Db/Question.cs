using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db;

public partial class Question
{

        public Question()
        {
            QuestionList = new HashSet<QuestionList>();
            Answers = new HashSet<Answer>();
        }

        [Display(Name = "ID pytania")]
        public int Id { get; set; }
        [Display(Name = "ID nauczyciela")]
        public int? TeacherId { get; set; }
        [Display(Name = "Kategoria pytania")]
        public int? CategoryId { get; set; }
        [Display(Name = "Typ pytania")]
        public int? TypeId { get; set; }
        [Display(Name = "Treść pytania")]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [StringLength(255, ErrorMessage = "Maksymalna długość to 255 znaków.")]
        [MinLength(5, ErrorMessage = "Pytanie musi mieć co najmnniej 5 znaków.")]
        public string? Text { get; set; }

        [Display(Name = "Kategoria pytania")]
        public virtual QuestionCategory? Category { get; set; }
        
        [Display(Name = "ID typu pytania")]
        public virtual QuestionType? Type { get; set; }
        public virtual ICollection<QuestionList> QuestionList { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }

    public override string ToString()
    {
        return $"Pytanie o id: {Id} - {Text}";
    }

}
