using System.IO;
namespace PriorMoney.DataImport.Interface
{
    public interface IReportFileChoseStrategy
    {
        FileSystemInfo Chose(DirectoryInfo directory);
    }
}