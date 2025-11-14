namespace ProjektInzynierski.Utils
{
    public class FileLogObserver : BaseObserver
    {
        private readonly string _filePath;
        public FileLogObserver(string filePath = "observer_logs.txt")
        => _filePath = filePath;


        public override void Update(string message)
        {
            File.AppendAllText(_filePath, $"{DateTime.Now:o}: {message}{Environment.NewLine}");
        }
    }
}