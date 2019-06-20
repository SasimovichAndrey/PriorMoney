using System.Threading.Tasks;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public abstract class BaseUserInterfaceCommand
    {
        public int MenuLevel { get; set; } = 1;
    }
}