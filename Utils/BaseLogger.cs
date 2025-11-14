using System;
using System.Collections.Generic;

namespace ProjektInzynierski.Utils
{
    //Podstawienie Liskov
    public abstract class BaseLogger : IAppLogger, ILogSubject
    {
        protected ILogStrategy _strategy;
        protected readonly List<ILogObserver> _observers = new();
        protected readonly List<string> _logs = new();
        protected int _count = 0;

        protected BaseLogger(ILogStrategy strategy)
        {
            _strategy = strategy;
        }

        public virtual void SetStrategy(ILogStrategy strategy)
        {
            _strategy = strategy;
        }

        public virtual void Attach(ILogObserver observer)
        {
            _observers.Add(observer);
        }

        public virtual void Detach(ILogObserver observer)
        {
            _observers.Remove(observer);
        }

        public virtual void Notify(string message)
        {
            foreach (var o in _observers)
                o.Update(message);
        }

        public virtual void Log(string message)
        {
            Log(message, LogLevel.INFO);
        }

        public virtual void Log(string message, LogLevel level)
        {
            _strategy.WriteLog(message, level);
            _logs.Add(message);
            _count++;
            Notify(message);
        }

        public virtual void ShowLogs()
        {
            Console.WriteLine("---- LOGS ----");

            foreach (var log in _logs)
                Console.WriteLine(log);
        }

        public virtual void ShowLogCount()
        {
            Console.WriteLine($"Jest {_count} logów.");
        }
    }
}
