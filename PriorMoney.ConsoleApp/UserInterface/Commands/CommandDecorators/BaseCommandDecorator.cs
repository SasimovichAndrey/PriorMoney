namespace PriorMoney.ConsoleApp.UserInterface.CommandDecorators
{
    public class BaseCommandDecorator
    {
        protected readonly IUserInterfaceCommand _command;
        protected readonly IUserInterfaceCommand<object> _commandReturningResult;

        public BaseCommandDecorator(IUserInterfaceCommand command)
        {
            _command = command;
        }

        public BaseCommandDecorator(IUserInterfaceCommand<object> command)
        {
            _commandReturningResult = command;
        }
    }
}