using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;

namespace PriorMoney.Storage.Managers
{
    public class DbLogicManager : IDbLogicManager
    {
        private readonly IStorage<CardOperation> _cardOperationStorage;

        public DbLogicManager(IStorage<CardOperation> cardOperationStorage)
        {
            _cardOperationStorage = cardOperationStorage;
        }

        public async Task AddOperationCategories(List<CardOperation> operations, List<string> categoryNames)
        {
            foreach (var operation in operations)
            {
                operation.Categories.UnionWith(categoryNames.Select(c => c.ToUpper().Trim()));
            }

            await _cardOperationStorage.UpdateManyAsync(operations, op => op.Categories);
        }

        public async Task<List<string>> GetOperationNameSuggestions(string originalName)
        {
            var operationsWithSameOriginalName = await _cardOperationStorage.Get(op => op.OriginalName == originalName);
            var suggestions = operationsWithSameOriginalName.GroupBy(op => op.UserDefinedName.Trim().ToUpper()).Select(g => g.First().UserDefinedName).ToList();

            return suggestions;
        }

        public async Task<List<CardOperation>> GetOperationsByCategories(List<string> categoryNames)
        {
            var upperCategoryNames = categoryNames.Select(c => c.ToUpper());
            var operations = await _cardOperationStorage.Get(op => op.Categories.Any(c => upperCategoryNames.Contains(c)));

            return operations;
        }

        public async Task<List<CardOperation>> SearchOperationsByName(string searchString)
        {
            var operations = await _cardOperationStorage.Get(op => op.UserDefinedName.ToUpper().Contains(searchString));

            return operations;
        }
    }
}