namespace ProjektInzynierski.Utils
{
    public enum LogLevel
    {
        INFO,
        ERROR,
        WARNING
    }
    public static class LogLevelExtensions
    {
        public static string ToLabel(this LogLevel level)
        {
            return level switch
            {
                LogLevel.INFO => "INFO",
                LogLevel.ERROR => "ERROR",
                LogLevel.WARNING => "WARNING",
                _ => "UNKNOWN"
            };
        }

    }


}
