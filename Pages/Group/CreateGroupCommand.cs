using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    public class CreateGroupCommand : IGroupCommand
    {
        private readonly DatabaseContext _context;
        private readonly string _nazwa;
        private readonly int _idNauczyciela;
        public Grupy Result { get; private set; }

        public CreateGroupCommand(DatabaseContext context, string nazwa, int idNauczyciela)
        {
            _context = context;
            _nazwa = nazwa;
            _idNauczyciela = idNauczyciela;
        }

        public void Execute()
        {
            var id = _context.Grupy.OrderByDescending(g => g.IdGrupy).FirstOrDefault()?.IdGrupy ?? 0;
            id++;

            var grupa = new Grupy
            {
                IdGrupy = id,
                Nazwa = _nazwa,
                IdNauczyciela = _idNauczyciela
            };

            _context.Grupy.Add(grupa);
            _context.SaveChanges();
            Result = grupa;
        }
    }
}