using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;
using System.Linq;
using System.Text;
using PriorMoney.Storage.Interface;
using System;

namespace PriorMoney.DataImport.CsvImport
{
    public class FileSystemOperationsLoader : ICardOperationsLoader
    {
        private readonly string _dataFolderPath;
        private readonly ICardOperationParser _cardOperationParser;
        private readonly IReportFileChoseStrategy _fileChoseStrategy;

        public FileSystemOperationsLoader(string dataFolderPath,
            ICardOperationParser cardOperationParser, 
            IReportFileChoseStrategy fileChoseStrategy)
        {
            _dataFolderPath = dataFolderPath;
            _cardOperationParser = cardOperationParser;
            _fileChoseStrategy = fileChoseStrategy;
        }

        public async Task<List<CardOperation>> LoadAsync()
        {
            var dataDir = new DirectoryInfo(_dataFolderPath);
            var file = _fileChoseStrategy.Chose(dataDir);

            if(file != null){
                CardOperation[] parsed;
                using (var reader = new StreamReader(file.FullName, Encoding.GetEncoding(1251)))
                {
                    var fileText = await reader.ReadToEndAsync();
                    parsed = _cardOperationParser.Parse(fileText);

                }

                MarkImportFileAsProcessed(file);
                    
                return parsed.ToList();
            }
            else{
                return new List<CardOperation>();
            }
        }

        private void MarkImportFileAsProcessed(FileSystemInfo file)
        {
            var newFileName = Path.Combine(Path.GetDirectoryName(file.FullName),
                Path.GetFileNameWithoutExtension(file.FullName) + "_imported" + Path.GetExtension(file.FullName)); 
            File.Move(file.FullName, newFileName);
        }
    }
}