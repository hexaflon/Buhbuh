using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class Test
    {
        public Test()
        {
            Questions = new HashSet<QuestionList>();
            Result = new HashSet<Result>();
        }

        [Display(Name = "ID testu")]
        public int Id { get; set; }
        [Display(Name = "ID grupy")]
        public int? GroupId { get; set; }
        [Display(Name = "ID nauczyciela")]
        public int? TeacherId { get; set; }
        [Display(Name = "Czy test jest widoczny?")]
        public bool IsVisible { get; set; }
        [Display(Name = "Data utworzenia")]
        public DateTime? CreationDate { get; set; }
        [Display(Name = "Data rozpoczęcia")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Data zakończenia")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Tytuł testu")]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [StringLength(90, ErrorMessage = "Maksymalna długość to 90 znaków.")]
        public string? Title { get; set; }
        [Display(Name = "Opis testu")]
        [StringLength(255, ErrorMessage = "Maksymalna długość to 255 znaków.")]
        public string? Description { get; set; }
        [Display(Name = "Czas trwania (min)")]
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [Range(5, 180, ErrorMessage = "Czas trwania testu musi wynosić od 5 minut do 3 godzin.")]
        public int? Duration { get; set; }
        [Display(Name = "ID grupy")]
        public virtual Group? Groups { get; set; }
        
        public virtual ICollection<QuestionList> Questions { get; set; }
        public virtual ICollection<Result> Result { get; set; }
        public override string ToString()
        {
            return $"Test o id: {Id} - {Title}";
        }
    }
}
