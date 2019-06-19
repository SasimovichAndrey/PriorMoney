using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriorMoney.ConsoleApp.UserInterface.Tools;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ImportCardOperationsCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        private readonly ICardOperationsLoader _operationsImporter;
        private readonly IModelStringView<CardOperation> _cardOperationView;

        public ImportCardOperationsCommand(ICardOperationsLoader cardOperationsImporter,
            IModelStringView<CardOperation> cardOperationView)
        {
            _operationsImporter = cardOperationsImporter;
            _cardOperationView = cardOperationView;
        }

        public async new Task ExecuteAsync()
        {
            var operations = await _operationsImporter.LoadAsync();

            RenderOperationsInfo(operations);

            List<CardOperation> operationsToSaveToStorage = operations.ToList();
            do
            {
                if (!AskUserIfEditIsNeeded()) break;

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

            SaveOperationsToStorage(operations);
        }

        private void SaveOperationsToStorage(List<CardOperation> operations)
        {
            throw new NotImplementedException();
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