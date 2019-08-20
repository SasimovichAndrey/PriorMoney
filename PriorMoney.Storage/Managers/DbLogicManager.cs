using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using PriorMoney.Utils;

namespace PriorMoney.Storage.Managers
{
    public class DbLogicManager : IDbLogicManager
    {
        private readonly IStorage<CardOperation> _cardOperationStorage;

        public DbLogicManager(IStorage<CardOperation> cardOperationStorage)
        {
            _cardOperationStorage = cardOperationStorage;
        }

        public async Task<CardOperation> AddNewOperation(CardOperation newCardOperation)
        {
            await _cardOperationStorage.Add(newCardOperation);

            return newCardOperation;
        }

        public async Task AddOperationCategories(List<CardOperation> operations, List<string> categoryNames)
        {
            foreach (var operation in operations)
            {
                operation.Categories.UnionWith(categoryNames.Select(c => c.ToUpper().Trim()));
            }

            await _cardOperationStorage.UpdateManyAsync(operations, op => op.Categories);
        }

        public async Task CreateOrUpdateOperations(List<CardOperation> editedOperations)
        {
            var toInsert = editedOperations.Where(op => op.Id == Guid.Empty);
            var toUpdate = editedOperations.Where(op => op.Id != Guid.Empty);

            var tasks = new List<Task>();
            if (toInsert.Any())
            {

                tasks.Add(_cardOperationStorage.AddMany(toInsert));
            }
            if (toUpdate.Any())
            {

                tasks.Add(_cardOperationStorage.UpdateManyAsync(toUpdate));
            }

            await Task.WhenAll(tasks);
        }

        public async Task<List<string>> GetAllCategories()
        {
            var categories = await _cardOperationStorage.GetDistinct(op => op.Categories);

            return categories;
        }

        public async Task<List<CardOperation>> GetLastOperations(int operationsNumber)
        {
            var operations = await _cardOperationStorage.GetAllAsQueryable().OrderBy(op => op.DateTime).Take(operationsNumber).ToListAsync();

            return operations;
        }

        public async Task<List<string>> GetOperationCategoriesSuggestions(string originalName)
        {
            var operationsWithSameOriginalName = await _cardOperationStorage.Get(op => op.OriginalName == originalName);
            var suggestions = new HashSet<string>(operationsWithSameOriginalName.SelectMany(op => op.Categories)).ToList();

            return suggestions;
        }

        public async Task<List<string>> GetOperationNameSuggestions(string originalName)
        {
            var operationsWithSameOriginalName = await _cardOperationStorage.Get(op => op.OriginalName == originalName);
            var suggestions = operationsWithSameOriginalName.GroupBy(op => op.UserDefinedName.Trim().ToUpper()).Select(g => g.First().UserDefinedName).ToList();

            return suggestions;
        }

        public async Task<List<CardOperation>> GetOperationsByAmountSpent(decimal min, decimal max)
        {
            var operations = await _cardOperationStorage.Get(op => op.Amount >= min && op.Amount <= max);

            return operations;
        }

        public async Task<List<CardOperation>> GetOperationsByCategories(List<string> categoryNames)
        {
            var upperCategoryNames = categoryNames.Select(c => c.ToUpper().Trim());
            var operations = await _cardOperationStorage.Get(op => op.Categories.Any(c => upperCategoryNames.Contains(c)));

            return operations;
        }

        public async Task<List<CardOperation>> GetOperationsByPeriod(DateTime startDate, DateTime endDate)
        {
            var operations = await _cardOperationStorage.Get(op => op.DateTime >= startDate && op.DateTime <= endDate);

            return operations;
        }

        public async Task<List<CardOperation>> GetOperationsWithNoCategories()
        {
            var operations = await _cardOperationStorage.Get(op => op.Categories.Count == 0 || op.Categories == null);

            return operations;
        }

        public async Task RemoveCardOperationById(Guid id)
        {
            await _cardOperationStorage.RemoveById(id);
        }

        public async Task<List<CardOperation>> SearchOperationsByFilter(Expression<Func<CardOperation, bool>> filter)
        {
            var operations = await _cardOperationStorage.GetAllAsQueryable().Where(filter).ToListAsync();

            return operations;
        }

        public async Task<List<CardOperation>> SearchOperationsByName(string searchString)
        {
            var operations = await _cardOperationStorage.Get(op => op.UserDefinedName.ToUpper().Contains(searchString));

            return operations;
        }
    }
}