using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.CommandDecorators;
using PriorMoney.ConsoleApp.UserInterface.Commands.Model;
using PriorMoney.Model;
using PriorMoney.Utils;

namespace PriorMoney.ConsoleApp.UserInterface.Commands.ManageOperations
{
    public class ManageOperationsUserInterfaceCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private List<CardOperation> _chosenCardOperations;
        private IServiceProvider _serviceProvider;
        private List<MenuCommandItem> _commands;


        public ManageOperationsUserInterfaceCommand(IServiceProvider serviceProvider)
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

            if (command is AddCardOperationToCategoryCommand && CheckIfOperationsHaveBeenChosen())
            {
                var addToCategoryCommand = command as AddCardOperationToCategoryCommand;

                await addToCategoryCommand.ExecuteAsync(_chosenCardOperations);
            }

            if (command is EditCardOperationsCommand && CheckIfOperationsHaveBeenChosen())
            {
                CheckIfOperationsHaveBeenChosen();
                var editOperationsCommand = command as EditCardOperationsCommand;

                await editOperationsCommand.ExecuteAsync(_chosenCardOperations);
            }

            if (command is IUserInterfaceCommand)
            {
                var regularUserInterfaceCommand = command as IUserInterfaceCommand;
                await regularUserInterfaceCommand.ExecuteAsync();
            }
        }

        private bool CheckIfOperationsHaveBeenChosen()
        {
            if (_chosenCardOperations == null || _chosenCardOperations.Count == 0)
            {
                Console.WriteLine("Сначала задайте операции для обработки");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void InitlizeCommandList(List<MenuCommandItem> commands)
        {
            var showAllCategoriesCommand = _serviceProvider.GetService<ShowAllCategoriesCommand>();
            commands.Add(new MenuCommandItem { Command = showAllCategoriesCommand, MenuItemLabel = "Показать все добавленные категории" });

            var editOperationsCommand = _serviceProvider.GetService<EditCardOperationsCommand>();
            commands.Add(new MenuCommandItem { Command = editOperationsCommand, MenuItemLabel = "Реадактировать операции" });

            var setCardOperationSetCommand = _serviceProvider.GetService<SetCardOperationSetCommand>();
            commands.Add(new MenuCommandItem { Command = setCardOperationSetCommand, MenuItemLabel = "Задать операции для обработки" });

            var addOperationsToCategoryCommand = _serviceProvider.GetService<AddCardOperationToCategoryCommand>();
            commands.Add(new MenuCommandItem { Command = addOperationsToCategoryCommand, MenuItemLabel = "Добавить операции к категории" });

            commands.Add(new MenuCommandItem { Command = new ExitCurrentMenuCommand(), MenuItemLabel = "Выход" });
        }
    }
}