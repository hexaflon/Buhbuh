namespace ProjektInzynierski.Utils
{
    public interface ILogging
    {
        void Log(string message);
        void Log(string message, LogLevel level);
    }
}