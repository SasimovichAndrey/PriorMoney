using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ManageOperationCategoriesUserInterfaceCommand : CompositeUserInterfaceCommand, IUserInterfaceCommand
    {
        public ManageOperationCategoriesUserInterfaceCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override void InitlizeCommandList(List<ValueTuple<IUserInterfaceCommand, string>> commands)
        {
            throw new NotImplementedException();
        }
    }
}