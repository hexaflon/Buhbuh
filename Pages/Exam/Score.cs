namespace ProjektInzynierski.Pages.Exam
{
    public class Score
    {
        public int? studentId { get; set; }
        public string message { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public double points { get; set; }
        public Score()
        {

        }
        public Score(int studentId, string message, string name, string surname, double points)
        {
            this.studentId = studentId;
            this.message = message;
            this.name = name;
            this.surname = surname;
            this.points = points;
        }

        public void newMessage(int maxPkt)
        {
            this.message = $"{points.ToString("0.##")}/{maxPkt} pkt.";
        }
    }
}
