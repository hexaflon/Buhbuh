

namespace ProjektInzynierski.Pages.Question
{
    using Microsoft.EntityFrameworkCore;
    using ProjektInzynierski.Pages.Answer;
    using ProjektInzynierski.Utils;
    using TestTest.Models.Db;
    public class QuestionFacade :
     IQuestionCreator,
     IQuestionReader,
     ITrueFalseAnswerGenerator
    {
        private readonly DatabaseContext _context;
        private readonly int _idTrueFalseQuestion;
        private Logger _logger;

        public QuestionFacade(DatabaseContext context)
        {
            _context = context;
            _idTrueFalseQuestion = _context.QuestionType
                .Where(tp => tp.Name.ToLower().Contains("prawda"))
                .Select(tp => tp.Id)
                .FirstOrDefault();
            _logger = Logger.getInstance();
        }

        public Question CreateQuestion(
            string text,
            int teacherId,
            int? categoryId,
            int questionType,
            bool isTrueFalse
        )
        {
            var pyt = new Question
            {
                Text = text,
                TeacherId = teacherId,
                CategoryId = categoryId,
                TypeId = questionType,
            };

            var id = _context.Question.OrderByDescending(p => p.Id).FirstOrDefault()?.Id ?? 0;
            pyt.Id = id + 1;

            _context.Question.Add(pyt);
            _context.SaveChanges();

            if (questionType == _idTrueFalseQuestion)
            {
                AddTrueFalseAnswers(pyt.Id, isTrueFalse);
            }

            return pyt;
        }

        public void AddTrueFalseAnswers(int questionId, bool isTrueFalse)
        {
            var id = _context.Answer.OrderByDescending(o => o.Id).FirstOrDefault()?.Id ?? 0;
            id++;

            var questionTextList = new List<string> { "Prawda", "Fałsz" };

            foreach (var questionText in questionTextList)
            {
                var odp = AnswerFactory.Create(
                    questionId: questionId,
                    text: questionText,
                    isCorrect: isTrueFalse,
                    AnswerId: id
                );
                id++;

                _context.Answer.Add(odp);
            }

            _context.SaveChanges();
        }

        public List<Question> GetQuestionList(string? searchText = null, int? categoryId = null, int? typeId = null)
        {
            var query = _context.Question
                .Include(p => p.Category)
                .Include(p => p.Type)
                .Include(p => p.Answers)
                .OrderByDescending(p => p.Id)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            if (typeId.HasValue)
            {
                query = query.Where(p => p.TypeId == typeId);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(p => p.Text.Contains(searchText));
            }

            var results = query.ToList();

            return results.OrderByDescending(p => p.Answers.FirstOrDefault()?.QuestionId).ToList();
        }
    }

}
