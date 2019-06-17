namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class BaseUserInterfaceCommand
    {
        public int MenuLevel { get; set; }

        public BaseUserInterfaceCommand(int menuLevel)
        {
            MenuLevel = menuLevel;
        }
    }
}