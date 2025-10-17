using ProjektInzynierski.Utils;
using TestTest.Models.Db;

namespace ProjektInzynierski.utils
{
    public class Logger : IAppLogger
    {
        private static Logger instance;
        private List<String> logList = new List<String>();
        private int logCount = 0;
        Logger(){}

        public static Logger getInstance()
        {
            if (Logger.instance == null)
            {
                Logger.instance = new Logger();
            }

            return Logger.instance;
        }

        public void Log(string message)
        {
            var logMessage = $"{DateTime.Now} : {message}";
            Console.WriteLine(logMessage);
            logList.Add(logMessage);
            logCount++;
        }

        public void ShowLogs()
        {
            Console.WriteLine("--------------------------------------------------------------------------------------------\n");
            ShowLogCount();
            foreach (var log in logList)
            {
                Console.WriteLine(log.ToString());
            }
        }
        public void ShowLogCount()
        {
            Console.WriteLine($"Jest {logCount} logów.");
        }
    }
}
