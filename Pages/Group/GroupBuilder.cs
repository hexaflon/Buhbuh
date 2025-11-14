using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    public class GroupBuilder
    {
        private TestTest.Models.Db.Group _group;


        public GroupBuilder()
        {
            _group = new TestTest.Models.Db.Group();
        }

        public GroupBuilder SetID(int id)
        {
            _group.Id = id;
            return this;
        }

        public GroupBuilder SetName(string name)
        {
            _group.Name = name;
            return this;
        }

        public GroupBuilder SetTeacherId(int teacherId)
        {
            _group.TeacherId = teacherId;
            return this;
        }
        public GroupBuilder AddParticipant(Participant participant)
        {
            _group.Participants.Add(participant);
            return this;
        }
        public GroupBuilder AddParticipantList(List<Participant> list)
        {
            foreach (Participant participant in list)
            {
                _group.Participants.Add(participant);
            }
            return this;
        }





        public TestTest.Models.Db.Group Build()
        {
            return _group;
        }
    }
}
