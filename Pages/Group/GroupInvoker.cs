namespace ProjektInzynierski.Pages.Group
{
    public class GroupInvoker
    {
        private readonly List<IGroupCommand> _commands = new List<IGroupCommand>();

        public void AddCommand(IGroupCommand command)
        {
            _commands.Add(command);
        }

        public void Run()
        {
            foreach (var command in _commands)
            {
                command.Execute();
            }
            _commands.Clear();
        }
    }
}
