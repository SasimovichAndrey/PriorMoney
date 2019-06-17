using System.Threading.Tasks;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ExitCurrentMenuCommand : BaseUserInterfaceCommand,  IUserInterfaceCommand
    {
        public ExitCurrentMenuCommand(int menuLevel) : base(menuLevel)
        {
        }

        public Task ExecuteAsync()
        {
            return Task.FromResult<object>(null);
        }
    }
}