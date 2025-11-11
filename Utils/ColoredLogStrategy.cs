namespace ProjektInzynierski.Utils
{
    public class ColoredLogStrategy : ILogStrategy
    {
        public void WriteLog(string message, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.WriteLine($"{DateTime.Now} [{level.ToLabel()}] {message}");
            Console.ResetColor();
        }
    }
}
