namespace ProjektInzynierski.Utils
{
    public abstract class BaseObserver : ILogObserver
    {
        public abstract void Update(string message);
    }
}