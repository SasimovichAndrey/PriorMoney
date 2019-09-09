using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriorMoney.DesktopApp.Fakes
{
    internal class FakeDbLogicManager : IDbLogicManager
    {
        private List<CardOperation> operations = new List<CardOperation>()
            {
                new CardOperation
                {
                    UserDefinedName = "First operation",
                    Amount = -30,
                    Currency = Currency.BYN,
                    DateTime = DateTime.Now.AddDays(-2),
                    Categories = new HashSet<string>() {"еда", "продукты"}
},
                new CardOperation
                {
                    UserDefinedName = "Second operation",
                    Amount = -35,
                    Currency = Currency.BYN,
                    DateTime = DateTime.Now.AddDays(-1),
                    Categories = new HashSet<string>() {"машина"}
                }
            };

        public Task AddManyOperations(List<CardOperation> operations)
        {
            throw new NotImplementedException();
        }

        public Task<CardOperation> AddNewOperation(CardOperation newCardOperation)
        {
            newCardOperation.Id = Guid.NewGuid();
            operations.Add(newCardOperation);

            return Task.FromResult(newCardOperation);
        }

        public Task AddOperationCategories(List<CardOperation> operations, List<string> categoryNames)
        {
            throw new NotImplementedException();
        }

        public Task CreateOrUpdateOperations(List<CardOperation> editedOperations)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public async Task<List<CardOperation>> GetLastOperations(int operationsNumber, int skip = 0)
        {
            await Task.Delay(100);

            return operations.Skip(skip).ToList();
        }

        public Task<List<string>> GetOperationCategoriesSuggestions(string originalName)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetOperationNameSuggestions(string originalName)
        {
            throw new NotImplementedException();
        }

        public Task<List<CardOperation>> GetOperationsByAmountSpent(decimal min, decimal max)
        {
            throw new NotImplementedException();
        }

        public Task<List<CardOperation>> GetOperationsByCategories(List<string> categoryNames)
        {
            throw new NotImplementedException();
        }

        public Task<List<CardOperation>> GetOperationsByPeriod(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<List<CardOperation>> GetOperationsWithNoCategories()
        {
            throw new NotImplementedException();
        }

        public Task RemoveCardOperationById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CardOperation>> SearchOperationsByFilter(Expression<Func<CardOperation, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<CardOperation>> SearchOperationsByName(string searchString)
        {
            throw new NotImplementedException();
        }
    }
}
