namespace ProjektInzynierski.Utils
{
    //Podstawienie Liskov
    public class ColoredLogStrategy : BaseLogStrategy
    {
        public override void WriteLog(string message, LogLevel level)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = level switch
            {
                LogLevel.ERROR => ConsoleColor.Red,
                LogLevel.WARNING => ConsoleColor.Yellow,
                _ => ConsoleColor.White
            };
            Console.WriteLine($"{DateTime.Now} [{level.ToLabel()}] {message}");
            Console.ForegroundColor = prev;
        }
    }
}