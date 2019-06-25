using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using PriorMoney.ConsoleApp.UserInterface.Commands;
using System.Linq;
using PriorMoney.DataImport.CsvImport;
using PriorMoney.Utils;
using PriorMoney.ConsoleApp.UserInterface.CommandDecorators;

namespace PriorMoney.ConsoleApp.UserInterface
{
    public class DefaultUserInterface : IConsoleAppUserInterface
    {
        private List<ValueTuple<IUserInterfaceCommand, string>> _commands;
        private readonly IServiceProvider _serviceProvider;

        public DefaultUserInterface(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InitlizeCommandList(menuLevel: 1);
        }

        private void InitlizeCommandList(int menuLevel)
        {
            _commands = new List<ValueTuple<IUserInterfaceCommand, string>>();

            var importCardOperationsCommand = _serviceProvider.GetService<ImportCardOperationsCommand>();
            importCardOperationsCommand.MenuLevel = menuLevel;
            _commands.Add((new EmptyLineCommandDecorator(importCardOperationsCommand, false, true), "Импорт операций"));

            var showOperationsMenuCommand = _serviceProvider.GetService<ShowOperationsCommand>();
            showOperationsMenuCommand.MenuLevel = menuLevel;
            _commands.Add((new EmptyLineCommandDecorator(showOperationsMenuCommand, false, true), "Показать операции за месяц"));

            var exitCurrentMenuCommand = _serviceProvider.GetService<ExitCurrentMenuCommand>();
            exitCurrentMenuCommand.MenuLevel = menuLevel;
            _commands.Add((exitCurrentMenuCommand, "Выход"));

        }

        public async Task StartAsync()
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