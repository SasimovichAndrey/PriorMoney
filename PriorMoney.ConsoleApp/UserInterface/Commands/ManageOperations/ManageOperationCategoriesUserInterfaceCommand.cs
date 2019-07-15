using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.CommandDecorators;
using PriorMoney.ConsoleApp.UserInterface.Commands.Model;
using PriorMoney.Model;
using PriorMoney.Utils;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ManageOperationCategoriesUserInterfaceCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private List<CardOperation> _chosenCardOperations;
        private IServiceProvider _serviceProvider;
        private List<MenuCommandItem> _commands;


        public ManageOperationCategoriesUserInterfaceCommand(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _commands = new List<MenuCommandItem>();

            InitlizeCommandList(_commands);
        }

        public async Task ExecuteAsync()
        {
            object command;
            do
            {
                RenderMenu(_commands);

                command = GetCommandFromUserInput(_commands);

                if (command != null)
                {
                    Console.Clear();
                    await ExecuteCommandInternal(command);
                }
                else
                {
                    Console.WriteLine("Попробуй ещё разок, дружок");
                }

            } while (!(command is ExitCurrentMenuCommand));
        }

        private async Task ExecuteCommandInternal(object command)
        {
            if (command is SetCardOperationSetCommand)
            {
                var setOperationsCommand = command as SetCardOperationSetCommand;
                _chosenCardOperations = await setOperationsCommand.ExecuteAsync();
            }

            if (command is AddCardOperationToCategoryCommand)
            {
                var addToCategoryCommand = command as AddCardOperationToCategoryCommand;

                await addToCategoryCommand.ExecuteAsync(_chosenCardOperations);
            }
        }

        private void InitlizeCommandList(List<MenuCommandItem> commands)
        {
            var setCardOperationSetCommand = _serviceProvider.GetService<SetCardOperationSetCommand>();
            commands.Add(new MenuCommandItem { Command = setCardOperationSetCommand, MenuItemLabel = "Задать операции для обработки" });

            var addOperationsToCategoryCommand = _serviceProvider.GetService<AddCardOperationToCategoryCommand>();
            commands.Add(new MenuCommandItem { Command = addOperationsToCategoryCommand, MenuItemLabel = "Добавить операции к категории" });

            commands.Add(new MenuCommandItem { Command = new ExitCurrentMenuCommand(), MenuItemLabel = "Выход" });
        }
    }
}