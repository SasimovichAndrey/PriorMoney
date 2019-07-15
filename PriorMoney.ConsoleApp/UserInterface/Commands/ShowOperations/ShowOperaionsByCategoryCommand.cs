using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;


namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ShowOperationsByCategoryCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private readonly IStorage<CardOperation> _cardOperationStorage;
        private readonly IModelStringView<CardOperation> _cardOperationStringView;
        private readonly IDbLogicManager _dbLogicManager;

        public ShowOperationsByCategoryCommand(IStorage<CardOperation> cardOPerationStorage, IModelStringView<CardOperation> cardOperationStringView,
            IDbLogicManager dbLogicManager)
        {
            _cardOperationStorage = cardOPerationStorage;
            _cardOperationStringView = cardOperationStringView;
            _dbLogicManager = dbLogicManager;
        }

        public async Task ExecuteAsync()
        {
            RenderMenu();
            var userCategoriesInput = ConsoleExtensions.ReadStringListOrRetry();
            var operations = await GetCardOperationsByUserCategoriesInput(userCategoriesInput);
            RenderOperationsAndTotal(operations);
        }

        private async Task<List<CardOperation>> GetCardOperationsByUserCategoriesInput(List<string> userCategoriesInput)
        {
            var operations = await _dbLogicManager.GetOperationsByCategories(userCategoriesInput);

            return operations;
        }

        private void RenderOperationsAndTotal(List<CardOperation> operations)
        {
            foreach (var op in operations)
            {
                Console.WriteLine($"{operations.IndexOf(op) + 1}. {_cardOperationStringView.GetView(op)}");
            }

            var total = operations.Sum(op => op.Amount);
            Console.WriteLine($"Всего потрачено/получено: {total}");
        }

        private void RenderMenu()
        {
            Console.WriteLine("Перечислите имена категорий через запятую");
        }
    }
}