namespace ProjektInzynierski.Utils
{
    public interface ILogStrategy
    {
        void WriteLog(string message, LogLevel level);
    }
}
