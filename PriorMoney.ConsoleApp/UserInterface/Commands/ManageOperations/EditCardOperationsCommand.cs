using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.Commands.Common;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;

namespace PriorMoney.ConsoleApp.UserInterface.Commands.ManageOperations
{
    class EditCardOperationsCommand : BaseUserInterfaceCommand, IParameterizableUserInterfaceCommand<List<CardOperation>>
    {
        private readonly IDbLogicManager _dbLogicManager;
        private readonly IModelStringView<CardOperation> _cardOperationStringView;

        public EditCardOperationsCommand(IDbLogicManager dbLogicManager, IModelStringView<CardOperation> cardOperationStringView)
        {
            this._dbLogicManager = dbLogicManager;
            this._cardOperationStringView = cardOperationStringView;
        }

        public async Task ExecuteAsync(List<CardOperation> operations)
        {
            var editedOperations = new List<CardOperation>();

            foreach (var operation in operations)
            {
                var editOperationCommand = new EditCardOperationUserInterfaceCommand(operation, _cardOperationStringView, _dbLogicManager);
                var editedOperation = await editOperationCommand.ExecuteAsync();
                editedOperations.AddRange(editedOperation);
            }

            await _dbLogicManager.CreateOrUpdateOperations(editedOperations);
        }
    }
}