namespace ProjektInzynierski.Utils
{
    public class ConsoleLogObserver : ILogObserver
    {
        public void Update(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[OBSERVER] New log: {message}");
            Console.ResetColor();
        }
    }

    public class FileLogObserver : ILogObserver
    {
        private readonly string _filePath = "observer_logs.txt";

        public void Update(string message)
        {
            File.AppendAllText(_filePath, $"{DateTime.Now}: {message}\n");
        }
    }
}
