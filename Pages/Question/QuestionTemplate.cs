using ProjektInzynierski.utils;
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
        public Pytanie CreateQuestionTemplate(string tresc,int idNauczyciela,int? idKategoria)
        {
            var pytanie = PrepareQuestion(tresc, idNauczyciela, idKategoria);
            SaveQuestion(pytanie);
            AddAnswers(pytanie);
            LogCreation(pytanie);
            return pytanie;
        }
        protected abstract Pytanie PrepareQuestion(string tresc, int idNauczyciela, int? idKategoria);
        protected abstract void AddAnswers(Pytanie pytanie);
        protected virtual void SaveQuestion(Pytanie pytanie)
        {
            var id = _context.Pytanie.OrderByDescending(p => p.IdPytanie).FirstOrDefault()?.IdPytanie ?? 0;
            pytanie.IdPytanie = id + 1;

            _context.Pytanie.Add(pytanie);
            _context.SaveChanges();
        }

        protected virtual void LogCreation(Pytanie pytanie)
        {
            _logger.Log($"Pytanie utworzone: {pytanie.Tresc}");
        }
    }
}
