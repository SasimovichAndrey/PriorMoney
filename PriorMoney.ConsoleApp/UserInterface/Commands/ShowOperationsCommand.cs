using System;
using System.Threading.Tasks;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ShowOperationsCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private readonly IStorage<CardOperation> _cardOperationsStorage;
        private readonly IModelStringView<CardOperation> _cardOperationStringView;

        public ShowOperationsCommand(IStorage<CardOperation> cardOperationsStorage,
            IModelStringView<CardOperation> cardOperationStringView)
        {
            _cardOperationsStorage = cardOperationsStorage;
            _cardOperationStringView = cardOperationStringView;
        }

        public async Task ExecuteAsync()
        {
            var currentDateTime = DateTime.Now;
            var monthAgoDateTime = currentDateTime.AddMonths(-1);
            var operations = await _cardOperationsStorage.Get(op => op.DateTime > monthAgoDateTime && op.DateTime < currentDateTime);

            Console.WriteLine($"Операции с {monthAgoDateTime} по {currentDateTime}");
            foreach (var op in operations)
            {
                Console.WriteLine(operations.IndexOf(op) + 1 + " " + _cardOperationStringView.GetView(op));
            }
        }
    }
}