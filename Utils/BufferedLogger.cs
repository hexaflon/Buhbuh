using ProjektInzynierski.Utils;
using System.Collections.Generic;

namespace ProjektInzynierski.Utils
{
    //Podstawienie Liskov
    public class BufferedLogger : BaseLogger
    {
        private readonly List<(string msg, LogLevel lvl)> _buffer = new();
        private readonly int _flushThreshold;

        public BufferedLogger(ILogStrategy strategy, int flushThreshold = 3)
            : base(strategy)
        {
            _flushThreshold = flushThreshold;
        }

        public override void Log(string message, LogLevel level)
        {
            _buffer.Add((message, level));

            if (_buffer.Count >= _flushThreshold)
                Flush();
        }

        private void Flush()
        {
            foreach (var (message, lvl) in _buffer)
                base.Log(message, lvl);

            _buffer.Clear();
        }
    }
}
