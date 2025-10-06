using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Pages.Answer;
using ProjektInzynierski.utils;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Question
{
    public class QuestionFacade
    {
        private readonly DatabaseContext _context;
        private readonly int _idTrueFalseQuestion;
        private Logger _logger;

        public QuestionFacade(DatabaseContext context)
        {
            _context = context;
            _idTrueFalseQuestion = _context.TypPytania
                .Where(tp => tp.Nazwa.ToLower().Contains("prawda"))
                .Select(tp => tp.IdTypPytania)
                .FirstOrDefault();
            _logger = Logger.getInstance();
        }

        public Pytanie CreateQuestion(
            string tresc,
            int idNauczyciela,
            int? idKategoria,
            int idTypPytania,
            bool isTrueFalse
            )
        {
            var pyt = new Pytanie
            {
                Tresc = tresc,
                IdNauczyciela = idNauczyciela,
                IdKategoriaPytania = idKategoria,
                IdTypPytania = idTypPytania,
            };

            var id = _context.Pytanie.OrderByDescending(p => p.IdPytanie).FirstOrDefault()?.IdPytanie ?? 0;
            pyt.IdPytanie = id + 1;

            _context.Pytanie.Add(pyt);
            _context.SaveChanges();
            Console.Write($"{idTypPytania} : {_idTrueFalseQuestion}\n");
            if (idTypPytania == _idTrueFalseQuestion)
            {
                AddTrueFalseAnswers(pyt.IdPytanie, isTrueFalse);
            }

            return pyt;

        }

        private void AddTrueFalseAnswers(int idPytania, bool isTrueFalse)
        {
            var id = _context.Odpowiedz.OrderByDescending(o => o.IdOdpowiedz).FirstOrDefault()?.IdOdpowiedz ?? 0;
            id++;

            var trescOdpList = new List<string> { "Prawda", "Fałsz" };

            foreach (var trescOdp in trescOdpList)
            {
                var odp = AnswerFactory.Create(
                    idPytanie:idPytania,
                    trescOdpowiedzi: trescOdp,
                    czyPoprawny: isTrueFalse,
                    idOdpowiedz: id
                    );
                id++;

                _context.Odpowiedz.Add( odp );
            }

            _context.SaveChanges();
        }


        public List<Pytanie> GetPytania(string? searchText = null, int? categoryId = null, int? typeId = null)
        {
            
            var query = _context.Pytanie
                .Include(p => p.IdKategoriaPytaniaNavigation)
                .Include(p => p.IdTypPytaniaNavigation)
                .Include(p => p.Odpowiedz)
                .OrderByDescending(p => p.IdPytanie)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.IdKategoriaPytania == categoryId);
            }

            if (typeId.HasValue)
            {
                query = query.Where(p => p.IdTypPytania == typeId);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(p => p.Tresc.Contains(searchText));
            }

            var results = query.ToList();



            return results.OrderByDescending(p => p.Odpowiedz.FirstOrDefault()?.IdPytanie).ToList();
        }


    }
}
