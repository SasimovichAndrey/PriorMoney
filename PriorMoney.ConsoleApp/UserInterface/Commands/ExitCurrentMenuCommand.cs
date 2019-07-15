using System.Threading.Tasks;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ExitCurrentMenuCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        public ExitCurrentMenuCommand()
        {
        }

        public async Task ExecuteAsync()
        {
            await Task.CompletedTask;
        }
    }
}