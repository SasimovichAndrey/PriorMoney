using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;

namespace PriorMoney.ConsoleApp.UserInterface.Commands.ManageOperations
{
    public class AddCardOperationToCategoryCommand : BaseUserInterfaceCommand, IParameterizableUserInterfaceCommand<List<CardOperation>>
    {
        private readonly IModelStringView<CardOperation> _cardOperationStringView;
        private readonly IDbLogicManager _dbLogicManager;

        public AddCardOperationToCategoryCommand(IModelStringView<CardOperation> cardOperationStringView, IDbLogicManager dbLogicManager)
        {
            _cardOperationStringView = cardOperationStringView;
            _dbLogicManager = dbLogicManager;
        }

        public async Task ExecuteAsync(List<CardOperation> operationsToAdd)
        {

            Console.WriteLine("Введите категории через запятую:");

            var categoryNames = ConsoleExtensions.ReadStringListOrRetry();
            await _dbLogicManager.AddOperationCategories(operationsToAdd, categoryNames);
        }
    }
}