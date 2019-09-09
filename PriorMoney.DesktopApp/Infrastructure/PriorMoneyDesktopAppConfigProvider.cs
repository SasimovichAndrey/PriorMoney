using PriorMoney.DataImport.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorMoney.DesktopApp.Infrastructure
{
    class PriorMoneyDesktopAppConfigProvider : IConfigurationProvider
    {
        public string GetImportDataFolderPath()
        {
            var reportDataFolderPath = ConfigurationManager.AppSettings["OperationsReportsDataFolderPath"];
            return reportDataFolderPath;
        }
    }
}
