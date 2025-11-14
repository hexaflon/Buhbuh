using ProjektInzynierski.Pages.Answer;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Question
{
    public class TrueFalseQuestion : QuestionTemplate
    {
        private readonly bool _isTrueFalse;
        private readonly int _idTrueFalse;

        public TrueFalseQuestion(DatabaseContext context, bool isTrueFalse) : base(context)
        {
            _isTrueFalse = isTrueFalse;
            _idTrueFalse = _context.QuestionType
                .Where(tp => tp.Name.ToLower().Contains("prawda"))
                .Select(tp => tp.Id)
                .FirstOrDefault();
        }

        protected override TestTest.Models.Db.Question PrepareQuestion(string text, int teacherId, int? categoryId)
        {
            return new TestTest.Models.Db.Question
            {
                Text = text,
                TeacherId = teacherId,
                CategoryId = categoryId,
                TypeId = _idTrueFalse
            };
        }

        protected override void AddAnswers(TestTest.Models.Db.Question question)
        {
            var id = _context.Answer.OrderByDescending(o => o.Id).FirstOrDefault()?.Id ?? 0;
            id++;
            var questionTextList = new List<string> { "Prawda", "Fałsz" };

            foreach (var questionText in questionTextList)
            {
                var odp = AnswerFactory.Create(
                    questionId: question.Id,
                    text: questionText,
                    isCorrect: _isTrueFalse,
                    AnswerId: id
                );
                id++;
                _context.Answer.Add(odp);
            }
            _context.SaveChanges();
        }
    }
}
