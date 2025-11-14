using Microsoft.EntityFrameworkCore;
using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    public class DeleteGroupCommand : IGroupCommand
    {
        private readonly DatabaseContext _context;
        private readonly int _groupId;

        public DeleteGroupCommand(DatabaseContext context, int groupId)
        {
            _context = context;
            _groupId = groupId;
        }

        public void Execute()
        {
            var group = _context.Group.Include(g => g.Participants).FirstOrDefault(g => g.Id == _groupId);
            if (group != null)
            {
                
                foreach (var participant in group.Participants.ToList())
                {
                    _context.Participant.Remove(participant);
                }

                _context.Group.Remove(group);
                _context.SaveChanges();
            }
        }
    }
}
