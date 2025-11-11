using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    public class DeleteGroupCommand : IGroupCommand
    {
        private readonly DatabaseContext _context;
        private readonly int _idGrupy;

        public DeleteGroupCommand(DatabaseContext context, int idGrupy)
        {
            _context = context;
            _idGrupy = idGrupy;
        }

        public void Execute()
        {
            var grupa = _context.Grupy.Include(g => g.Uczestnicy).FirstOrDefault(g => g.IdGrupy == _idGrupy);
            if (grupa != null)
            {
                
                foreach (var uczestnik in grupa.Uczestnicy.ToList())
                {
                    _context.Uczestnicy.Remove(uczestnik);
                }

                _context.Grupy.Remove(grupa);
                _context.SaveChanges();
            }
        }
    }
}
