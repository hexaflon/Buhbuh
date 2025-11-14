using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class Result
    {
        public Result()
        {
            QuestionResult = new HashSet<QuestionResult>();
        }

        [Display(Name = "Rozwiązanie")]
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public int? TestId { get; set; }
        [Display(Name = "Liczba punktów")]
        public double? Points { get; set; }

        [Display(Name = "Tytuł testu")]
        public virtual Test? Tests { get; set; }
        
        public virtual ICollection<QuestionResult> QuestionResult { get; set; }
    }
}
