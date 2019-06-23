using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ImportCardOperationsCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private readonly ICardOperationsLoader _operationsImporter;
        private readonly IModelStringView<CardOperation> _cardOperationView;
        private readonly IStorage<CardOperation> _cardOperationStorage;
        private readonly IStorage<CardOperationsImport> _cardOperationsImportStorage;

        public ImportCardOperationsCommand(ICardOperationsLoader cardOperationsImporter,
            IModelStringView<CardOperation> cardOperationView,
            IStorage<CardOperation> cardOperationStorage,
            IStorage<CardOperationsImport> cardOperationsImportStorage)
        {
            _operationsImporter = cardOperationsImporter;
            _cardOperationView = cardOperationView;
            _cardOperationStorage = cardOperationStorage;
            _cardOperationsImportStorage = cardOperationsImportStorage;
        }

        public async Task ExecuteAsync()
        {
            var operations = await _operationsImporter.LoadAsync();

            if (operations.Count > 0)
            {
                await ImportOperations(operations);
            }
            else
            {
                Console.WriteLine("Не найдено операций для импорта");
            }
        }

        private async Task ImportOperations(List<CardOperation> operations)
        {
            RenderOperationsInfo(operations);

            List<CardOperation> operationsToSaveToStorage = operations.ToList();
            do
            {
                if (!AskUserIfEditIsNeeded())
                {
                    operationsToSaveToStorage.ForEach(op => op.UserDefinedName = op.UserDefinedName ?? op.OriginalName);
                    break;
                }

                operationsToSaveToStorage = new List<CardOperation>();
                foreach (var operation in operations)
                {
                    var editOpearationCommand = new EditCardOperationUserInterfaceCommand(2, operation, _cardOperationView);
                    var editedOperation = await editOpearationCommand.ExecuteAsync();
                    operationsToSaveToStorage.Add(editedOperation);
                }

                Console.WriteLine("Операции после редактирования:");
                RenderOperationsInfo(operationsToSaveToStorage);
            } while (true);

            var import = SaveImportToStorage();
            SaveOperationsToStorage(operationsToSaveToStorage, import);
        }

        private CardOperationsImport SaveImportToStorage()
        {
            var newImport = new CardOperationsImport
            {
                Date = DateTime.Now
            };

            _cardOperationsImportStorage.Add(newImport);

            return newImport;
        }

        private void SaveOperationsToStorage(List<CardOperation> operations, CardOperationsImport import)
        {
            operations.ForEach(op => op.ImportId = import.Id);

            _cardOperationStorage.AddMany(operations);
        }

        private void RenderOperationsInfo(List<CardOperation> operations)
        {
            foreach (var op in operations)
            {
                Console.WriteLine(_cardOperationView.GetView(op));
            }
        }

        private bool AskUserIfEditIsNeeded()
        {
            return ConsoleExtensions.AskYesNo("Редактировать перед сохраннением?");
        }
    }
}