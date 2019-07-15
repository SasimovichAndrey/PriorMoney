using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.Commands.Model;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using PriorMoney.Utils;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ShowOperationsCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private readonly IStorage<CardOperation> _cardOperationsStorage;
        private readonly IModelStringView<CardOperation> _cardOperationStringView;
        private readonly List<MenuCommandItem> _menuItems;

        public ShowOperationsCommand(IStorage<CardOperation> cardOperationsStorage,
            IModelStringView<CardOperation> cardOperationStringView, IServiceProvider serviceProvider)
        {
            _cardOperationsStorage = cardOperationsStorage;
            _cardOperationStringView = cardOperationStringView;
            _menuItems = new List<MenuCommandItem>()
            {
                new MenuCommandItem{ Command = serviceProvider.GetService<ShowOperationsByPeriodCommand>(), MenuItemLabel = "Показать операции за период"},
                new MenuCommandItem{ Command = serviceProvider.GetService<ShowOperationsByCategoryCommand>(), MenuItemLabel = "Показать операции по категории"},
                new MenuCommandItem{ Command = serviceProvider.GetService<SearchForOperationsCommand>(), MenuItemLabel = "Поиск попераций"}
            };
        }

        public async Task ExecuteAsync()
        {
            RenderMenu(_menuItems);
            var command = GetCommandFromUserInput(_menuItems);
            await ExecuteCommandInternal(command);
        }

        private async Task ExecuteCommandInternal(object command)
        {
            if (command is IUserInterfaceCommand)
            {
                await (command as IUserInterfaceCommand).ExecuteAsync();
            }
        }
    }
}