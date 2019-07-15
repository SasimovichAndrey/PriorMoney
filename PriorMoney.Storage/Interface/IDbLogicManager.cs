using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.Model;

namespace PriorMoney.Storage.Interface
{
    public interface IDbLogicManager
    {
        Task AddOperationCategories(List<CardOperation> operations, List<string> categoryNames);
        Task<List<CardOperation>> GetOperationsByCategories(List<string> categoryNames);
        Task<List<string>> GetOperationNameSuggestions(string originalName);
        Task<List<CardOperation>> SearchOperationsByName(string searchString);
    }
}