using System.IO;
namespace PriorMoney.DataImport.Interface
{
    public interface IReportFileChoseStrategy
    {
        string Chose(DirectoryInfo directory);
    }
}