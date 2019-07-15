using System;
using System.Threading.Tasks;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using System.Collections.Generic;
using PriorMoney.Utils;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class SetCardOperationSetCommand : BaseUserInterfaceCommand, IUserInterfaceCommand<List<CardOperation>>
    {
        private readonly IStorage<CardOperation> _cardOperationStorage;
        private readonly IModelStringView<CardOperation> _cardOperationStringView;
        private List<Tuple<string, Func<Task<List<CardOperation>>>>> _cardOperationRetriveHandlers = new List<Tuple<string, Func<Task<List<CardOperation>>>>>();

        public SetCardOperationSetCommand()
        {
        }

        public SetCardOperationSetCommand(IStorage<CardOperation> cardOperationStorage, IModelStringView<CardOperation> cardOperationStringView)
        {
            _cardOperationStorage = cardOperationStorage;
            _cardOperationStringView = cardOperationStringView;

            _cardOperationRetriveHandlers.Add(new Tuple<string, Func<Task<List<CardOperation>>>>(
                "За месяц",
                async () => await _cardOperationStorage.Get(op => op.DateTime > DateTime.Now.AddMonths(-1))));
            _cardOperationRetriveHandlers.Add(new Tuple<string, Func<Task<List<CardOperation>>>>(
                "За два месяца",
                async () => await _cardOperationStorage.Get(op => op.DateTime > DateTime.Now.AddMonths(-2))));
            _cardOperationRetriveHandlers.Add(new Tuple<string, Func<Task<List<CardOperation>>>>(
                "Последние 10",
                async () => await _cardOperationStorage.Get(op => true, 10, op => op.DateTime)));
        }

        public async Task<List<CardOperation>> ExecuteAsync()
        {
            RenderOperationRangeRetrieveMenu();
            var userRangeChoice = ConsoleExtensions.ReadIntOrRetry();
            var operations = await GetCardOperationsBasedOnUsersChoice(userRangeChoice);
            RenderOperations(operations);
            var chosenOperations = AskUserToChoseOperationsFromSet(operations);

            return chosenOperations;
        }

        private List<CardOperation> AskUserToChoseOperationsFromSet(List<CardOperation> operations)
        {
            Console.WriteLine("Перечислите операции к обработке:");

            var chosenOperationsNumbers = ConsoleExtensions
                .ReadIntListOrRetry()
                .SelectUnique();

            var chosenOperations = new List<CardOperation>();
            foreach (var operationNumber in chosenOperationsNumbers)
            {
                if (operationNumber > 0 && operationNumber <= operations.Count)
                {
                    var operation = operations[operationNumber - 1];
                    chosenOperations.Add(operation);
                }
            }

            return chosenOperations;
        }

        private void RenderOperations(List<CardOperation> operations)
        {
            foreach (var op in operations)
            {
                Console.WriteLine($"{operations.IndexOf(op) + 1}. {_cardOperationStringView.GetView(op)}");
            }
        }


        private async Task<List<CardOperation>> GetCardOperationsBasedOnUsersChoice(int userChoice)
        {
            return await _cardOperationRetriveHandlers[userChoice].Item2.Invoke();
        }

        private void RenderOperationRangeRetrieveMenu()
        {
            Console.WriteLine("Выбор операций из бд:");

            for (int i = 0; i < _cardOperationRetriveHandlers.Count; i++)
            {
                var handler = _cardOperationRetriveHandlers[i];
                Console.WriteLine($"{i}. {handler.Item1}");
            }
        }
    }
}
