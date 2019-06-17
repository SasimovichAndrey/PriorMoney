using System.Threading.Tasks;
using PriorMoney.Model;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class EditCardOperationUserInterfaceCommand : BaseUserInterfaceCommand, IUserInterfaceCommand<CardOperation>
    {
        private readonly CardOperation operation;

        public EditCardOperationUserInterfaceCommand(int menuLevel, CardOperation operation) : base(menuLevel)
        {
            this.operation = operation;
        }

        public Task<CardOperation> ExecuteAsync()
        {
            throw new System.NotImplementedException();
        }

        Task IUserInterfaceCommand.ExecuteAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}