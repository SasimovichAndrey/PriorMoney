using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;


namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class SearchForOperationsCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private readonly IModelStringView<CardOperation> _cardOperationStringView;
        private readonly IDbLogicManager _dbLogicManager;
        private readonly List<MenuItemHandler> _menuHandlers;

        public SearchForOperationsCommand(IStorage<CardOperation> cardOPerationStorage, IModelStringView<CardOperation> cardOperationStringView,
            IDbLogicManager dbLogicManager)
        {
            _cardOperationStringView = cardOperationStringView;
            _dbLogicManager = dbLogicManager;

            _menuHandlers = new List<MenuItemHandler>()
            {
                new MenuItemHandler{MenuLabel = "По имени", Handler =  this.SearchByName}
            };
        }

        public async Task ExecuteAsync()
        {
            RenderMenu();
            var handler = GetHandlerByUserInput();
            var operations = await handler.Invoke();
            RenderOperations(operations);
        }

        private void RenderOperations(List<CardOperation> operations)
        {
            if (operations.Any())
            {
                foreach (var operation in operations)
                {
                    Console.WriteLine($"{_cardOperationStringView.GetView(operation)}");
                }
            }
            else
            {
                Console.WriteLine("Не найдено операций по запросу");
            }
        }

        private Func<Task<List<CardOperation>>> GetHandlerByUserInput()
        {
            var userInput = ConsoleExtensions.ReadIntOrRetry(max: _menuHandlers.Count);

            return _menuHandlers[userInput - 1].Handler;
        }

        private void RenderMenu()
        {
            for (int i = 0; i < _menuHandlers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_menuHandlers[i].MenuLabel}");
            }
        }

        private async Task<List<CardOperation>> SearchByName()
        {
            Console.Write("Строка поиска:");
            var searchString = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(searchString))
            {
                Console.WriteLine("Строка запроса не может быть пустой");
                searchString = Console.ReadLine();
            }

            var operations = await _dbLogicManager.SearchOperationsByName(searchString);

            return operations;
        }
    }

    internal class MenuItemHandler
    {
        public string MenuLabel { get; set; }
        public Func<Task<List<CardOperation>>> Handler { get; set; }
    }
}