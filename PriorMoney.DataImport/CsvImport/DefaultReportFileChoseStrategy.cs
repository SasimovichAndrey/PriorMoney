using System.IO;
using System.Linq;
using PriorMoney.DataImport.Interface;

namespace PriorMoney.DataImport.CsvImport
{
    public class DefaultReportFileChoseStrategy : IReportFileChoseStrategy
    {
        public FileSystemInfo Chose(DirectoryInfo directory)
        {
            return directory.GetFileSystemInfos().First();
        }
    }
}