using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.Model;

namespace PriorMoney.DataImport.Interface
{
    public interface ICardOperationsImporter
    {
         Task<List<CardOperation>> ImportAsync();
    }
}