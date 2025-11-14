using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    public class CreateGroupCommand : IGroupCommand
    {
        private readonly DatabaseContext _context;
        private readonly string _name;
        private readonly int _teacherId;
        public TestTest.Models.Db.Group Result { get; private set; }

        public CreateGroupCommand(DatabaseContext context, string name, int teacherId)
        {
            _context = context;
            _name = name;
            _teacherId = teacherId;
        }

        public void Execute()
        {
            var id = _context.Group.OrderByDescending(g => g.Id).FirstOrDefault()?.Id ?? 0;
            id++;

            var group = new TestTest.Models.Db.Group
            {
                Id = id,
                Name = _name,
                TeacherId = _teacherId
            };

            _context.Group.Add(group);
            _context.SaveChanges();
            Result = group;
        }
    }
}