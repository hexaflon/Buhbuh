namespace ProjektInzynierski.Utils
{
    public interface IAppLogger
    {
        void Log(string message);
        void Log(string message, Utils.LogLevel level);
        void ShowLogs();
        void ShowLogCount();
        public void SetStrategy(ILogStrategy logStrategy);
    }
}
