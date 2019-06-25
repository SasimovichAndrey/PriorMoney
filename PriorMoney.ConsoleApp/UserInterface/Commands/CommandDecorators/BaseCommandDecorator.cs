namespace PriorMoney.ConsoleApp.UserInterface.CommandDecorators
{
    public class BaseCommandDecorator
    {
        protected readonly IUserInterfaceCommand _command;

        public BaseCommandDecorator(IUserInterfaceCommand command)
        {
            _command = command;
        }

        public int MenuLevel { get => _command.MenuLevel; set => _command.MenuLevel = value; }
    }
}