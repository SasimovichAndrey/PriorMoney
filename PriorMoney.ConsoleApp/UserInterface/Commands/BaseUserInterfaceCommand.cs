using System.Threading.Tasks;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public abstract class BaseUserInterfaceCommand
    {
        public int MenuLevel { get; set; }

        public BaseUserInterfaceCommand(int menuLevel)
        {
            MenuLevel = menuLevel;
        }

        public virtual Task ExecuteAsync()
        {
            return ExecuteAsync();
        }
    }
}