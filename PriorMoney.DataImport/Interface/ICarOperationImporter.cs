using PriorMoney.Model;

namespace PriorMoney.DataImport.Interface
{
    public interface ICarOperationImporter<TDataSourceType>
    {
        CardOperation[] Import(TDataSourceType dataSource);
    }
}
