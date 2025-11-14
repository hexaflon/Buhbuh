namespace ProjektInzynierski.Utils
{
    //Podstawienie Liskov
    public class SimpleLogStrategy : BaseLogStrategy
    {
        public override void WriteLog(string message, LogLevel level)
        {
            Console.WriteLine($"{DateTime.Now} [{level.ToLabel()}] {message}");
        }
    }
}