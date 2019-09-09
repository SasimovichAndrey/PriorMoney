using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PriorMoney.Model;

namespace PriorMoney.Storage.Interface
{
    public interface IDbLogicManager
    {
        Task AddOperationCategories(List<CardOperation> operations, List<string> categoryNames);
        Task<List<CardOperation>> GetOperationsByCategories(List<string> categoryNames);
        Task<List<CardOperation>> GetOperationsWithNoCategories();
        Task<List<string>> GetOperationNameSuggestions(string originalName);
        Task<List<string>> GetOperationCategoriesSuggestions(string originalName);
        Task<List<CardOperation>> SearchOperationsByName(string searchString);
        Task<List<string>> GetAllCategories();
        Task CreateOrUpdateOperations(List<CardOperation> editedOperations);
        Task<List<CardOperation>> GetOperationsByAmountSpent(decimal min, decimal max);
        Task<List<CardOperation>> GetOperationsByPeriod(DateTime startDate, DateTime endDate);
        Task<List<CardOperation>> GetLastOperations(int operationsNumber, int skip = 0);
        Task<List<CardOperation>> SearchOperationsByFilter(Expression<Func<CardOperation, bool>> filter);
        Task<CardOperation> AddNewOperation(CardOperation newCardOperation);
        Task RemoveCardOperationById(Guid id);
        Task AddManyOperations(List<CardOperation> operations);
    }
}