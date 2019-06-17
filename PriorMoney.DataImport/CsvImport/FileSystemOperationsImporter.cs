using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;

namespace PriorMoney.DataImport.CsvImport
{
    public class FileSystemOperationsImporter : ICardOperationsImporter
    {
        private readonly string _dataFolderPath;

        public FileSystemOperationsImporter(string dataFolderPath, ICarOperationParser cardOperationParser)
        {
            _dataFolderPath = dataFolderPath;
        }

        public async Task<List<CardOperation>> ImportAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}