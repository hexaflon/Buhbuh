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
            _idTrueFalse = _context.TypPytania
                .Where(tp => tp.Nazwa.ToLower().Contains("prawda"))
                .Select(tp => tp.IdTypPytania)
                .FirstOrDefault();
        }

        protected override Pytanie PrepareQuestion(string tresc, int idNauczyciela, int? idKategoria)
        {
            return new Pytanie
            {
                Tresc = tresc,
                IdNauczyciela = idNauczyciela,
                IdKategoriaPytania = idKategoria,
                IdTypPytania = _idTrueFalse
            };
        }

        protected override void AddAnswers(Pytanie pytanie)
        {
            var id = _context.Odpowiedz.OrderByDescending(o => o.IdOdpowiedz).FirstOrDefault()?.IdOdpowiedz ?? 0;
            id++;
            var trescOdpList = new List<string> { "Prawda", "Fałsz" };

            foreach (var trescOdp in trescOdpList)
            {
                var odp = AnswerFactory.Create(
                    idPytanie: pytanie.IdPytanie,
                    trescOdpowiedzi: trescOdp,
                    czyPoprawny: _isTrueFalse,
                    idOdpowiedz: id
                );
                id++;
                _context.Odpowiedz.Add(odp);
            }
            _context.SaveChanges();
        }
    }
}
