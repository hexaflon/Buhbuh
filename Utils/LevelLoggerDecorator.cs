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

        public void Log(string message)
        {
            if(_level == LogLevelExtensions.ToLabel(LogLevel.ERROR)) Console.ForegroundColor = ConsoleColor.Red;
            _logger.Log($"[{_level}] {message}");
            if(_level != LogLevelExtensions.ToLabel(LogLevel.INFO))Console.ResetColor();
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
