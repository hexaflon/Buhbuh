using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTest.Models.Db
{
    public partial class QuestionResult
    {
        public int Id { get; set; }
        public int? AnswerId { get; set; }
        public int? ResultId { get; set; }

        public virtual Answer? Answers { get; set; }
        public virtual Result? Results { get; set; }
    }
}
