namespace ProjektInzynierski.Utils
{
    public class LevelLoggerDecorator : IAppLogger
    {
        private readonly IAppLogger _logger;
        private readonly string _level;
        public LevelLoggerDecorator(IAppLogger logger, string level)
        {
            _logger = logger;
            _level = level;
        }
        public void Log(string message, Utils.LogLevel level) { }
        public void Log(string message)
        {
            if (Enum.TryParse(_level, true, out LogLevel level))
            {
                _logger.Log(message, level);
                
            }
            else
            {
                _logger.Log(message, LogLevel.INFO);
            }
        }

        public void SetStrategy(ILogStrategy logStrategy)
        {
            _logger.SetStrategy(logStrategy);
        }


        public void ShowLogs()
        { 
            _logger.ShowLogs(); 
        }
        public void ShowLogCount()
        {  
            _logger.ShowLogCount(); 
        }

    }
}
