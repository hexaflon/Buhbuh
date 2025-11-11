using ProjektInzynierski.Utils;
using TestTest.Models.Db;

namespace ProjektInzynierski.utils
{
    public class Logger : IAppLogger
    {
        private static Logger instance;
        private List<String> logList = new List<String>();
        private int logCount = 0;

        private ILogStrategy logStrategy;
        private List<ILogObserver> observers = new();

        private Logger(){
            logStrategy = new SimpleLogStrategy();
        }

        public static Logger getInstance()
        {
            if (Logger.instance == null)
            {
                Logger.instance = new Logger();
            }

            return Logger.instance;
        }

        public void SetStrategy(ILogStrategy logStrategy)
        {
            this.logStrategy = logStrategy;
        }

        public void Log(string message)
        {
            logStrategy.WriteLog(message, Utils.LogLevel.INFO);
            logList.Add(message);
            logCount++;
            Notify(message);
        }
        public void Log(string message, Utils.LogLevel level)
        {
            logStrategy.WriteLog(message, level);
            logList.Add(message);
            logCount++;
            Notify(message);
        }

        public void Attach(ILogObserver observer) 
        { 
            observers.Add(observer); 
        }
        public void Detach(ILogObserver observer) 
        { 
            observers.Remove(observer); 
        }

        public void Notify(string message)
        {
            foreach (var observer in observers)
                observer.Update(message);
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
