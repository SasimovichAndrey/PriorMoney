using System.IO;
using Microsoft.Win32;
using PriorMoney.DataImport.Interface;

namespace PriorMoney.DesktopApp.Infrastructure
{
    internal class FileOpenDialogtReportFileChoseStrategy : IReportFileChoseStrategy
    {
        public string Chose(DirectoryInfo directory)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = directory.FullName;
            if (openFileDialog.ShowDialog() == true)
            {
                var fileName = openFileDialog.FileName;
                return fileName;
            }

            return null;
        }
    }
}