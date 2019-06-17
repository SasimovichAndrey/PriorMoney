using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using PriorMoney.ConsoleApp.UserInterface.Commands;
using System.Linq;
using PriorMoney.DataImport.CsvImport;

namespace PriorMoney.ConsoleApp.UserInterface
{
    public class DefaultUserInterface : IConsoleAppUserInterface
    {
        private List<ValueTuple<IUserInterfaceCommand, string>> _commands;

        public DefaultUserInterface()
        {
            _commands = new List<ValueTuple<IUserInterfaceCommand, string>>();

            _commands.Add((new ImportCardOperationsCommand(1, new FileSystemOperationsImporter("", new CsvCardOperationParser())), "Импорт операций"));
            _commands.Add((new ExitCurrentMenuCommand(1), "Выход"));
        }
        
        public async Task StartAsync()
        {
            IUserInterfaceCommand command;
            do
            {
                RenderMenu();

                command = GetCommandFromUserInput();

                if(command != null)
                {
                    Console.Clear();
                    await command.ExecuteAsync();
                }
                else
                {
                    Console.WriteLine("Попробуй ещё разок, дружок");
                }
                
            } while(!(command is ExitCurrentMenuCommand));
        }

        private IUserInterfaceCommand GetCommandFromUserInput()
        {
            var userInput = Console.ReadLine();

            int parsedUserInput;
            if(int.TryParse(userInput, out parsedUserInput))
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
            for(int i = 0; i < _commands.Count(); i++)
            {
                Console.WriteLine($"{i}. {_commands[i].Item2}");
            }
        }
    }
}