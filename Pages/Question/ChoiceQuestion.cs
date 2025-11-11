using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Question
{
    public class ChoiceQuestion : QuestionTemplate
    {
        private readonly int _typeId;

        public ChoiceQuestion(DatabaseContext context, int typeId) : base(context)
        {
            _typeId = typeId;
        }

        protected override Pytanie PrepareQuestion(string tresc, int idNauczyciela, int? idKategoria)
        {
            return new Pytanie
            {
                Tresc = tresc,
                IdNauczyciela = idNauczyciela,
                IdKategoriaPytania = idKategoria,
                IdTypPytania = _typeId
            };
        }

        protected override void AddAnswers(Pytanie pytanie)
        {
        }
    }
}
