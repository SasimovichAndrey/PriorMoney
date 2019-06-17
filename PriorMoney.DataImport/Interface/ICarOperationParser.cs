using PriorMoney.Model;

namespace PriorMoney.DataImport.Interface
{
    public interface ICarOperationParser
    {
        CardOperation[] Import(string dataPath);
    }
}
