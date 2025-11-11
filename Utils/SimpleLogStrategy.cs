namespace ProjektInzynierski.Utils
{
    public class SimpleLogStrategy:ILogStrategy
    {
        public void WriteLog(string message, LogLevel level)
        {
            Console.WriteLine($"{DateTime.Now} [{level.ToLabel()}] {message}");
        }
    }
}
