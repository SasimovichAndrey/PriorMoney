using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;


namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ShowOperationsByPeriodCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private readonly IStorage<CardOperation> _cardOperationStorage;
        private readonly IModelStringView<CardOperation> _cardOperationStringView;
        private List<Tuple<string, Func<Task<List<CardOperation>>>>> _cardOperationRetriveHandlers = new List<Tuple<string, Func<Task<List<CardOperation>>>>>();

        public ShowOperationsByPeriodCommand(IStorage<CardOperation> cardOPerationStorage, IModelStringView<CardOperation> cardOperationStringView)
        {
            _cardOperationStorage = cardOPerationStorage;
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

        public async Task ExecuteAsync()
        {
            RenderOperationRangeRetrieveMenu();
            var userRangeChoice = ConsoleExtensions.ReadIntOrRetry();
            var operations = await GetCardOperationsBasedOnUsersChoice(userRangeChoice);
            RenderOperations(operations);
        }

        private async Task<List<CardOperation>> GetCardOperationsBasedOnUsersChoice(int userChoice)
        {
            return await _cardOperationRetriveHandlers[userChoice - 1].Item2.Invoke();
        }

        private void RenderOperations(List<CardOperation> operations)
        {
            foreach (var op in operations)
            {
                Console.WriteLine($"{operations.IndexOf(op) + 1}. {_cardOperationStringView.GetView(op)}");
            }
        }

        private void RenderOperationRangeRetrieveMenu()
        {
            Console.WriteLine("Выбор операций из бд:");

            for (int i = 0; i < _cardOperationRetriveHandlers.Count; i++)
            {
                var handler = _cardOperationRetriveHandlers[i];
                Console.WriteLine($"{i + 1}. {handler.Item1}");
            }
        }
    }
}