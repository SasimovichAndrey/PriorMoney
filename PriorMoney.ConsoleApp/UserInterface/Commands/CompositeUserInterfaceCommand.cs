using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.CommandDecorators;
using PriorMoney.Utils;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public abstract class CompositeUserInterfaceCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {

        private List<ValueTuple<IUserInterfaceCommand, string>> _commands;
        protected readonly IServiceProvider _serviceProvider;

        public CompositeUserInterfaceCommand(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InitlizeCommandListInternal();
        }

        protected virtual void InitlizeCommandListInternal()
        {
            _commands = new List<ValueTuple<IUserInterfaceCommand, string>>();

            this.InitlizeCommandList(_commands);

            var exitCurrentMenuCommand = _serviceProvider.GetService<ExitCurrentMenuCommand>();
            _commands.Add((exitCurrentMenuCommand, "Выход"));

        }

        protected abstract void InitlizeCommandList(List<ValueTuple<IUserInterfaceCommand, string>> commands);

        public async Task ExecuteAsync()
        {
            IUserInterfaceCommand command;
            do
            {
                RenderMenu();

                command = GetCommandFromUserInput();

                if (command != null)
                {
                    Console.Clear();
                    await command.ExecuteAsync();
                }
                else
                {
                    Console.WriteLine("Попробуй ещё разок, дружок");
                }

            } while (!(command is ExitCurrentMenuCommand));
        }

        private IUserInterfaceCommand GetCommandFromUserInput()
        {
            var userInput = Console.ReadLine();

            int parsedUserInput;
            if (int.TryParse(userInput, out parsedUserInput))
            {
                return _commands[parsedUserInput].Item1;
            }
            else
            {
                return null;
            }
        }

        private void RenderMenu()
        {
            for (int i = 0; i < _commands.Count(); i++)
            {
                Console.WriteLine($"{i}. {_commands[i].Item2}");
            }
        }
    }
}