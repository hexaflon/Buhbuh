using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class QuestionList
    {
        [Display(Name = "ID listy pytań")]
        public int Id { get; set; }
        public int? TestId { get; set; }
        public int? QuestionId { get; set; }

        public virtual Question? Questions { get; set; }
        public virtual Test? Tests { get; set; }
    }
}
