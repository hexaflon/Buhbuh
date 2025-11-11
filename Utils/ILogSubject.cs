namespace ProjektInzynierski.Utils
{
    public interface ILogSubject
    {
        void Attach(ILogObserver observer);
        void Detach(ILogObserver observer);
        void Notify(string message);
    }
}
