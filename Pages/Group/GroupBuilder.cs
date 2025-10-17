using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Group
{
    public class GroupBuilder
    {
        private Grupy _group;


        public GroupBuilder()
        {
            _group = new Grupy();
        }

        public GroupBuilder SetID(int id)
        {
            _group.IdGrupy = id;
            return this;
        }

        public GroupBuilder SetNazwa(string nazwa)
        {
            _group.Nazwa = nazwa;
            return this;
        }

        public GroupBuilder SetNauczyciel(int idNauczyciel)
        {
            _group.IdNauczyciela = idNauczyciel;
            return this;
        }
        public GroupBuilder AddUczestnik(Uczestnicy uczestnik)
        {
            _group.Uczestnicy.Add(uczestnik);
            return this;
        }
        public GroupBuilder AddListUczestnik(List<Uczestnicy> list)
        {
            foreach (Uczestnicy uczestnik in list)
            {
                _group.Uczestnicy.Add(uczestnik);
            }
            return this;
        }





        public Grupy Build()
        {
            return _group;
        }
    }
}
