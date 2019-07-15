using System;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface;

namespace PriorMoney.ConsoleApp.UserInterface.CommandDecorators
{
    public class EmptyLineCommandDecorator : BaseCommandDecorator, IUserInterfaceCommand
    {
        private readonly bool _insertLineBefore;
        private readonly bool _insertLineAfter;

        public EmptyLineCommandDecorator(IUserInterfaceCommand command, bool insertLineBefore, bool insertLineAfter) : base(command)
        {
            _insertLineBefore = insertLineBefore;
            _insertLineAfter = insertLineAfter;
        }

        public EmptyLineCommandDecorator(IUserInterfaceCommand<object> command, bool insertLineBefore, bool insertLineAfter) : base(command)
        {
            _insertLineBefore = insertLineBefore;
            _insertLineAfter = insertLineAfter;
        }

        public async Task ExecuteAsync()
        {
            if (_insertLineBefore) Console.WriteLine();

            await _command.ExecuteAsync();

            if (_insertLineAfter) Console.WriteLine();
        }


    }
}