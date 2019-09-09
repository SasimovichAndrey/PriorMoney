using System.IO;
using System.Linq;
using PriorMoney.DataImport.Interface;

namespace PriorMoney.DataImport.CsvImport
{
    public class DefaultReportFileChoseStrategy : IReportFileChoseStrategy
    {
        public string Chose(DirectoryInfo directory)
        {
            return directory.GetFileSystemInfos().FirstOrDefault(f => !Path.GetFileNameWithoutExtension(f.Name).EndsWith("_imported"))?.FullName;
        }
    }
}