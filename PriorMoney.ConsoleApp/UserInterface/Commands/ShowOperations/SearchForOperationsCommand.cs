using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.Model;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;


namespace PriorMoney.ConsoleApp.UserInterface.Commands.ShowOperations
{
    public class SearchForOperationsCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private readonly IModelStringView<CardOperation> _cardOperationStringView;
        private readonly IDbLogicManager _dbLogicManager;
        private readonly IModelStringView<OperationSetStatistics> _cardOperationSetStatisticsStringView;
        private readonly List<MenuItemHandler> _menuHandlers;

        public SearchForOperationsCommand(IStorage<CardOperation> cardOPerationStorage, IModelStringView<CardOperation> cardOperationStringView,
            IDbLogicManager dbLogicManager, IModelStringView<OperationSetStatistics> cardOperationSetStatisticsStringView)
        {
            _cardOperationStringView = cardOperationStringView;
            _dbLogicManager = dbLogicManager;
            _cardOperationSetStatisticsStringView = cardOperationSetStatisticsStringView;
            _menuHandlers = new List<MenuItemHandler>()
            {
                new MenuItemHandler{MenuLabel = "По имени", Handler =  this.SearchByName},
                new MenuItemHandler{MenuLabel = "По сумме потраченого.", Handler =  this.SearchByAmountSpent}
            };
        }

        private async Task<List<CardOperation>> SearchByAmountSpent()
        {
            Console.Write("Сумма не менее чем:");
            var minToCompare = ConsoleExtensions.ReadDecimalOrRetry();
            if (minToCompare > 0)
            {
                minToCompare = -minToCompare;
            }

            Console.Write("Сумма не более чем:");
            var maxToCompare = ConsoleExtensions.ReadDecimalOrRetry();
            if (maxToCompare > 0)
            {
                maxToCompare = -maxToCompare;
            }
            if (maxToCompare == 0)
            {
                maxToCompare = decimal.MinValue;
            }

            var operations = await _dbLogicManager.GetOperationsByAmountSpent(maxToCompare, minToCompare); // min and max switched cuz we're looking for spent operations

            return operations;
        }

        public async Task ExecuteAsync()
        {
            RenderMenu();
            var handler = GetHandlerByUserInput();
            var operations = await handler.Invoke();
            var statistics = new OperationSetStatistics(operations);
            RenderOperations(operations, statistics);
        }

        private void RenderOperations(List<CardOperation> operations, OperationSetStatistics statistics)
        {
            if (operations.Any())
            {
                foreach (var operation in operations)
                {
                    Console.WriteLine(_cardOperationStringView.GetView(operation));
                }

                Console.Write(_cardOperationSetStatisticsStringView.GetView(statistics));
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