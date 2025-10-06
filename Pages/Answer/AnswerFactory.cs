using TestTest.Models.Db;

namespace ProjektInzynierski.Pages.Answer
{
    public static class AnswerFactory
    {
        public static Odpowiedz Create(int idPytanie, string trescOdpowiedzi,bool czyPoprawny, int idOdpowiedz)
        {
            var odpowiedz = new Odpowiedz();
            odpowiedz.TrescOdpowiedzi = trescOdpowiedzi;
            odpowiedz.IdPytanie = idPytanie;
            odpowiedz.IdOdpowiedz = idOdpowiedz;

            switch (trescOdpowiedzi)
            {
                //Dla pytań Prawda/Fałsz argument czyPoprawny można interpretować jako:
                //czy Prawda to poprawna odpowiedź
                case "Prawda":
                    {
                        if (czyPoprawny)odpowiedz.CzyPoprawny = true;
                        else { odpowiedz.CzyPoprawny = false; }
                        break;
                    }
                case "Fałsz":
                    {
                        if (czyPoprawny) odpowiedz.CzyPoprawny = false;
                        else odpowiedz.CzyPoprawny = true;
                        break;
                    }
                default:
                    {
                        odpowiedz.CzyPoprawny = czyPoprawny;
                        break;
                    }
            }
            
            return odpowiedz;
        }
    }
}
