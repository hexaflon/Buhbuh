using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    public class AddMemberCommand : IGroupCommand
    {
        private readonly DatabaseContext _context;
        private readonly int _GroupId;
        private readonly int _StudentId;

        public AddMemberCommand(DatabaseContext context, int groupId, int studentId)
        {
            _context = context;
            _GroupId = groupId;
            _StudentId = studentId;
        }

        public void Execute()
        {
            int nextId = (_context.Participant.OrderByDescending(u => u.Id).FirstOrDefault()?.Id ?? 0) + 1;

            var participant = new Participant
            {
                Id = nextId,
                GroupId = _GroupId,
                StudentId = _StudentId
            };

            _context.Participant.Add(participant);
            _context.SaveChanges();
        }
    }
}
