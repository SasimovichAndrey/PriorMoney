using PriorMoney.ConsoleApp.Model;
using PriorMoney.DataImport.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace PriorMoney.ConsoleApp.Infrastructure
{
    internal class ConfigProvider : IConfigurationProvider
    {
        private readonly ConsoleAppConfig _cfg;

        public ConfigProvider(ConsoleAppConfig cfg)
        {
            _cfg = cfg;
        }

        public string GetImportDataFolderPath()
        {
            return _cfg.OperationsReportsDataFolderPath;
        }
    }
}
