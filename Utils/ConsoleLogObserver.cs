namespace ProjektInzynierski.Utils
{
    public class ConsoleLogObserver : BaseObserver
    {
        public override void Update(string message)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[OBSERVER] {message}");
            Console.ForegroundColor = prev;
        }
    }
}