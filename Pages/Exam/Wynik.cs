namespace ProjektInzynierski.Pages.Exam
{
    public class Wynik
    {
        public int? idUcznia { get; set; }
        public string wiadomosc { get; set; }
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public double punkty { get; set; }
        public Wynik()
        {

        }
        public Wynik(int idUcznia, string wiadomosc, string imie, string nazwisko, double punkty)
        {
            this.idUcznia = idUcznia;
            this.wiadomosc = wiadomosc;
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.punkty = punkty;
        }

        public void nowaWiadomosc(int maxPkt)
        {
            this.wiadomosc = $"{punkty.ToString("0.##")}/{maxPkt} pkt.";
        }
    }
}
