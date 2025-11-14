namespace ProjektInzynierski.Utils
{
    //Podstawienie Liskov
    public class Logger : BaseLogger
    {
        private static Logger instance;


        private readonly List<string> _logList = new();
        private int _logCount = 0;
        private BaseLogStrategy _logStrategy = new SimpleLogStrategy();
        private readonly List<ILogObserver> _observers = new();


        private Logger():base(new SimpleLogStrategy()) { }


        public static Logger getInstance()
        {
            if (instance == null)
                instance = new Logger();
            return instance;
        }


        public override void SetStrategy(ILogStrategy strategy)
        {
            _logStrategy = (BaseLogStrategy)strategy;
        }


        public override void Log(string message)
        {
            Log(message, LogLevel.INFO);
        }


        public override void Log(string message, LogLevel level)
        {
            _logStrategy.WriteLog(message, level);
            _logList.Add($"[{level.ToLabel()}] {message}");
            _logCount++;
            Notify(message);
        }


        public override void Attach(ILogObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }


        public override void Detach(ILogObserver observer)
        {
            _observers.Remove(observer);
        }


        public override void Notify(string message)
        {
            foreach (var obs in _observers)
                obs.Update(message);
        }


        public override void ShowLogs()
        {
            Console.WriteLine("---- LOGS ----");
            ShowLogCount();
            foreach (var log in _logList)
                Console.WriteLine(log);
            Console.WriteLine("----------------");
        }


        public override void ShowLogCount()
        {
            Console.WriteLine($"Jest {_logCount} logów.");
        }
    }
}