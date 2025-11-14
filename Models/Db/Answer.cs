using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class Answer
    {
        public Answer()
        {
            QuestionResults = new HashSet<QuestionResult>();
        }

        [Display(Name = "ID odpowiedzi")]
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        [Display(Name = "Odpowiedzi")]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [StringLength(255, ErrorMessage = "Maksymalna długość to 255 znaków.")]
        public string Text { get; set; } = null!;
        [Display(Name = "Czy odpowiedź jest poprawna?")]
        public bool IsCorrect { get; set; }

        public virtual Question? Questions { get; set; }
        public virtual ICollection<QuestionResult> QuestionResults { get; set; }
        public override string ToString()
        {
            return $"Odpowiedz o id: {Id}, Pytania o id: {QuestionId} - {Text}";
        }
    }
}
