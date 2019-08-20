using System;
using System.Linq;
using System.Threading.Tasks;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using System.Collections.Generic;
using PriorMoney.Utils;
using System.Linq.Expressions;

namespace PriorMoney.ConsoleApp.UserInterface.Commands.ManageOperations
{
    public class SetCardOperationSetCommand : BaseUserInterfaceCommand, IUserInterfaceCommand<List<CardOperation>>
    {
        private readonly IStorage<CardOperation> _cardOperationStorage;
        private readonly IModelStringView<CardOperation> _cardOperationStringView;
        private readonly IDbLogicManager _dbLogicManager;
        private List<Tuple<string, Func<Task<List<CardOperation>>>>> _cardOperationRetriveHandlers = new List<Tuple<string, Func<Task<List<CardOperation>>>>>();

        public SetCardOperationSetCommand(IStorage<CardOperation> cardOperationStorage, IModelStringView<CardOperation> cardOperationStringView,
            IDbLogicManager dbLogicManager)
        {
            _cardOperationStorage = cardOperationStorage;
            _cardOperationStringView = cardOperationStringView;
            _dbLogicManager = dbLogicManager;

            _cardOperationRetriveHandlers.Add(new Tuple<string, Func<Task<List<CardOperation>>>>(
                "За месяц",
                async () => await _cardOperationStorage.Get(op => op.DateTime > DateTime.Now.AddMonths(-1))));
            _cardOperationRetriveHandlers.Add(new Tuple<string, Func<Task<List<CardOperation>>>>(
                "За два месяца",
                async () => await _cardOperationStorage.Get(op => op.DateTime > DateTime.Now.AddMonths(-2))));
            _cardOperationRetriveHandlers.Add(new Tuple<string, Func<Task<List<CardOperation>>>>(
                "Последние 10",
                async () => await _cardOperationStorage.Get(op => true, 10, op => op.DateTime)));
            _cardOperationRetriveHandlers.Add(new Tuple<string, Func<Task<List<CardOperation>>>>(
                "По категориям",
                GetByCategory));
            _cardOperationRetriveHandlers.Add(new Tuple<string, Func<Task<List<CardOperation>>>>(
                "По имени",
                GetByName));
            _cardOperationRetriveHandlers.Add(new Tuple<string, Func<Task<List<CardOperation>>>>(
                "По сумме",
                GetByAmount));
        }

        private async Task<List<CardOperation>> GetByCategory()
        {
            Console.WriteLine("Перечислите имена категорий через запятую");

            var userCategoriesInput = ConsoleExtensions.ReadStringListOrRetry();

            List<CardOperation> operations = null;
            if (userCategoriesInput.Count != 0)
            {

                operations = await _dbLogicManager.GetOperationsByCategories(userCategoriesInput);
            }
            else
            {
                operations = await _dbLogicManager.GetOperationsWithNoCategories();
            }

            return operations;
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
                .ReadIntRangeListOrRetry()
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
            var operations = await _cardOperationRetriveHandlers[userChoice - 1].Item2.Invoke();
            operations = operations.OrderBy(op => op.DateTime).ToList();

            return operations;
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

        private async Task<List<CardOperation>> GetByName()
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

        private async Task<List<CardOperation>> GetByAmount()
        {
            var amountFilterString = AskForAmountFilter();
            var filterExpression = CreateFilterExpressionForAmountFilter(amountFilterString);
            var operations = await _dbLogicManager.SearchOperationsByFilter(filterExpression);

            return operations;
        }

        private string AskForAmountFilter()
        {
            Console.Write("Введите сумму (наприме >100, <100, 100):");
            var amountFilterString = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(amountFilterString))
            {
                Console.WriteLine("Строка запроса не может быть пустой");
                amountFilterString = Console.ReadLine();
            }

            return amountFilterString;
        }

        private Expression<Func<CardOperation, bool>> CreateFilterExpressionForAmountFilter(string amountFilterString)
        {
            var filterSign = "=";
            if(amountFilterString.StartsWith("<") || amountFilterString.StartsWith(">") || amountFilterString.StartsWith("="))
            {
                filterSign = amountFilterString.Substring(0, 1);
                amountFilterString = amountFilterString.Substring(1, amountFilterString.Length - 1);
            }
            
            var filterDecimal = 0m;
            if(!decimal.TryParse(amountFilterString.Trim(), out filterDecimal))
            {
                throw new ApplicationException("Не удалось распознать фильтр");
            }

            Expression<Func<CardOperation, bool>> expression = null;
            switch (filterSign)
            {
                case "=":
                    return (CardOperation op) => op.Amount == filterDecimal;
                case "<":
                    return (CardOperation op) => op.Amount < filterDecimal;
                case ">":
                    return (CardOperation op) => op.Amount > filterDecimal;
            }

            return expression;
        }
    }
}
