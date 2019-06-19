using System;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using PriorMoney.Model;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class EditCardOperationUserInterfaceCommand : BaseUserInterfaceCommand, IUserInterfaceCommand<CardOperation>
    {
        private readonly CardOperation _operation;
        private readonly IModelStringView<CardOperation> _operationStringView;

        public EditCardOperationUserInterfaceCommand(int menuLevel, CardOperation operation, IModelStringView<CardOperation> operationStringView)
        {
            this._operation = operation;
            this._operationStringView = operationStringView;
        }

        public new Task<CardOperation> ExecuteAsync()
        {
            Console.WriteLine($"Операция: {_operationStringView.GetView(_operation)}");

            var newOperation = _operation.Clone();
            if (ConsoleExtensions.AskYesNo("Редактировать?"))
            {

                EditProperty("Имя операции", _operation.Name, (value) => { newOperation.Name = value; });
                EditProperty("Дата", _operation.DateTime, (value) => { DateTime parsedValue; if (DateTime.TryParse(value, out parsedValue)) newOperation.DateTime = parsedValue; });
                EditProperty("Потрачено / получено", _operation.Amount, (value) => { decimal parsedValue; if (decimal.TryParse(value, out parsedValue)) newOperation.Amount = parsedValue; });

                return Task.FromResult(newOperation);
            }
            else
            {
                return Task.FromResult(newOperation);
            }
        }

        private void EditProperty(string propName, object propValue, Action<string> setPropFunc)
        {
            Console.Write($"{propName} ({propValue.ToString()}): ");
            var propValueStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(propValueStr))
            {
                setPropFunc(propValueStr);
            }
        }
    }
}