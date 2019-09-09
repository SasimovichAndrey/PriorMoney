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
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ICardOperationParser _cardOperationParser;
        private readonly IReportFileChoseStrategy _fileChoseStrategy;

        public FileSystemOperationsLoader(IConfigurationProvider configurationProvider,
            ICardOperationParser cardOperationParser,
            IReportFileChoseStrategy fileChoseStrategy)
        {
            _configurationProvider = configurationProvider;
            _cardOperationParser = cardOperationParser;
            _fileChoseStrategy = fileChoseStrategy;
        }

        public async Task<List<CardOperation>> LoadAsync()
        {
            var dataDir = new DirectoryInfo(_configurationProvider.GetImportDataFolderPath());
            var fileFullName = _fileChoseStrategy.Chose(dataDir);

            if (fileFullName != null)
            {
                CardOperation[] parsed;
                using (var reader = new StreamReader(fileFullName, Encoding.GetEncoding(1251)))
                {
                    var fileText = await reader.ReadToEndAsync();
                    parsed = _cardOperationParser.Parse(fileText);

                }

                MarkImportFileAsProcessed(fileFullName);

                return parsed.ToList();
            }
            else
            {
                return new List<CardOperation>();
            }
        }

        private void MarkImportFileAsProcessed(string fileFullName)
        {
            var newFileName = Path.Combine(Path.GetDirectoryName(fileFullName),
                DateTime.Now.ToString("ddMMyyyy_hhmmss") + "_imported" + Path.GetExtension(fileFullName));
            File.Move(fileFullName, newFileName);
        }
    }
}