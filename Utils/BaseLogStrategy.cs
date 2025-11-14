namespace ProjektInzynierski.Utils
{
    //Podstawienie Liskov
    public abstract class BaseLogStrategy : ILogStrategy
    {
        public abstract void WriteLog(string message, LogLevel level);
    }
}