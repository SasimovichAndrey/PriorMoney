using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ImportCardOperationsCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private readonly ICardOperationsImporter _operationsImporter;

        public ImportCardOperationsCommand(int menuLevel, ICardOperationsImporter cardOperationsImporter) : base(menuLevel)
        {
            MenuLevel = menuLevel;
            _operationsImporter = cardOperationsImporter;
        }

        public async Task ExecuteAsync()
        {
            var operations = await _operationsImporter.ImportAsync();

            RenderOperationsInfo(operations);

            var editOperationsBeforeSaveIsNeeded = AskUserIfEditIsNeeded();

            List<CardOperation> operationsToSaveToStorage;
            if(editOperationsBeforeSaveIsNeeded)
            {
                operationsToSaveToStorage = new List<CardOperation>();
                foreach(var operation in operations)
                {
                    var editedOperation = await (new EditCardOperationUserInterfaceCommand(2, operation)).ExecuteAsync();
                    operationsToSaveToStorage.Add(editedOperation);
                }
            }
            else
            {
                operationsToSaveToStorage = operations;
            }

            SaveOperationsToStorage(operations);
        }

        private void SaveOperationsToStorage(List<CardOperation> operations)
        {
            throw new NotImplementedException();
        }

        private void RenderOperationsInfo(object operations)
        {
            throw new NotImplementedException();
        }

        private bool AskUserIfEditIsNeeded()
        {
            throw new NotImplementedException();
        }
    }
}