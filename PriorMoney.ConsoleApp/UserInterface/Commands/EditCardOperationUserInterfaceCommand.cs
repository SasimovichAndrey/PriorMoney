using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using System.Linq;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class EditCardOperationUserInterfaceCommand : BaseUserInterfaceCommand, IUserInterfaceCommand<List<CardOperation>>
    {
        private readonly CardOperation _operation;
        private readonly IModelStringView<CardOperation> _operationStringView;
        private readonly IDbLogicManager _dbLogicManager;

        public EditCardOperationUserInterfaceCommand(int menuLevel, CardOperation operation, IModelStringView<CardOperation> operationStringView,
            IDbLogicManager dbLogicManager)
        {
            this._operation = operation;
            this._operationStringView = operationStringView;
            this._dbLogicManager = dbLogicManager;
        }

        public async Task<List<CardOperation>> ExecuteAsync()
        {
            Console.WriteLine($"Операция: {_operationStringView.GetView(_operation)}");

            var resultOperationsList = new List<CardOperation>();
            int resultOperationsCount = 1;

            if (ConsoleExtensions.AskYesNo("Разбить на несколько?"))
            {
                Console.WriteLine("Введите количество операций, на которое надо разбить эту");
                resultOperationsCount = ConsoleExtensions.ReadIntOrRetry();
            }

            var moneyLeft = _operation.Amount;
            for (int i = 0; i < resultOperationsCount; i++)
            {
                var newOperation = _operation.Clone();
                newOperation.Amount = moneyLeft;

                // Get suggestions for operation name property based on existing operations in DB with the same original name
                var operationNameSuggestions = await _dbLogicManager.GetOperationNameSuggestions(_operation.OriginalName);

                Console.WriteLine($"Операция: {_operationStringView.GetView(newOperation)}");
                if (ConsoleExtensions.AskYesNo("Редактировать?"))
                {
                    EditProperty("Имя операции", newOperation.OriginalName, (value) => { newOperation.UserDefinedName = value; }, operationNameSuggestions);
                    EditProperty("Дата", newOperation.DateTime, (value) => { DateTime parsedValue; if (DateTime.TryParse(value, out parsedValue)) newOperation.DateTime = parsedValue; });
                    EditOperationAmount(newOperation);
                }

                resultOperationsList.Add(newOperation);
                moneyLeft -= newOperation.Amount;
            }

            return resultOperationsList;
        }

        private void EditOperationAmount(CardOperation newOperation)
        {
            bool repeat;
            var oldAmount = newOperation.Amount;
            do
            {
                repeat = false;
                EditProperty("Потрачено / получено", newOperation.Amount, (value) => { decimal parsedValue; if (decimal.TryParse(value, out parsedValue)) newOperation.Amount = parsedValue; });

                if (newOperation.Amount > 0 && oldAmount < 0
                    || newOperation.Amount < 0 && oldAmount > 0)
                {
                    var allIsOk = ConsoleExtensions.AskYesNo("Поменять знак операции?");
                    if (!allIsOk)
                    {
                        newOperation.Amount = oldAmount;
                        repeat = true;
                    }
                }
            } while (repeat);
        }

        private void EditProperty(string propName, object propValue, Action<string> setPropFunc, List<string> valueSuggestions = null)
        {
            if (valueSuggestions != null && valueSuggestions.Any())
            {
                Console.WriteLine($"Предложения: {string.Join(" | ", valueSuggestions)}");
            }
            Console.Write($"{propName} ({propValue.ToString()}): ");
            var propValueStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(propValueStr))
            {
                setPropFunc(propValueStr);
            }
        }
    }
}