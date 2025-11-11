using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    public class AddMemberCommand : IGroupCommand
    {
        private readonly DatabaseContext _context;
        private readonly int _idGrupy;
        private readonly int _idUcznia;

        public AddMemberCommand(DatabaseContext context, int idGrupy, int idUcznia)
        {
            _context = context;
            _idGrupy = idGrupy;
            _idUcznia = idUcznia;
        }

        public void Execute()
        {
            int nextId = (_context.Uczestnicy.OrderByDescending(u => u.IdUczestnicy).FirstOrDefault()?.IdUczestnicy ?? 0) + 1;

            var uczestnik = new Uczestnicy
            {
                IdUczestnicy = nextId,
                IdGrupy = _idGrupy,
                IdUcznia = _idUcznia
            };

            _context.Uczestnicy.Add(uczestnik);
            _context.SaveChanges();
        }
    }
}
