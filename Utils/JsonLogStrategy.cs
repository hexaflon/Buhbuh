namespace ProjektInzynierski.Utils
{
    //Podstawienie Liskov
    public class JsonLogStrategy : BaseLogStrategy
    {
        public override void WriteLog(string message, LogLevel level)
        {
            var json = $"{{\"time\":\"{DateTime.Now:o}\",\"level\":\"{level.ToLabel()}\",\"message\":\"{message.Replace("\"", "\\\"")}\"}}";
            Console.WriteLine(json);
        }
    }
}