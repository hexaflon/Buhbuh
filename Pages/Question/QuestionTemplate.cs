
using ProjektInzynierski.Utils;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Question
{
    public abstract class QuestionTemplate
    {
        protected readonly DatabaseContext _context;
        protected readonly Logger _logger;

        protected QuestionTemplate(DatabaseContext context)
        {
            _context = context;
            _logger = Logger.getInstance();
        }
        public TestTest.Models.Db.Question CreateQuestionTemplate(string text,int teacherId,int? categoryId)
        {
            var question = PrepareQuestion(text, teacherId, categoryId);
            SaveQuestion(question);
            AddAnswers(question);
            LogCreation(question);
            return question;
        }
        protected abstract TestTest.Models.Db.Question PrepareQuestion(string text, int teacherId, int? categoryId);
        protected abstract void AddAnswers(TestTest.Models.Db.Question question);
        protected virtual void SaveQuestion(TestTest.Models.Db.Question question)
        {
            var id = _context.Question.OrderByDescending(p => p.Id).FirstOrDefault()?.Id ?? 0;
            question.Id = id + 1;

            _context.Question.Add(question);
            _context.SaveChanges();
        }

        protected virtual void LogCreation(TestTest.Models.Db.Question question)
        {
            _logger.Log($"Pytanie utworzone: {question.Text}");
        }
    }
}
