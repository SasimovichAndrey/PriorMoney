using System.IO;
using System.Threading.Tasks;
using PriorMoney.Model;

namespace PriorMoney.DataImport.Interface
{
    public interface ICardOperationParser
    {
        CardOperation[] Parse(string csvString);
    }
}
