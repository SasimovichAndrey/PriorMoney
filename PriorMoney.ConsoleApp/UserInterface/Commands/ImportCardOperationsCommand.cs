using System;
using System.Threading.Tasks;

namespace PriorMoney.ConsoleApp.UserInterface.Commands
{
    public class ImportCardOperationsCommand : BaseUserInterfaceCommand, IUserInterfaceCommand
    {
        public ImportCardOperationsCommand(int menuLevel) : base(menuLevel)
        {
            MenuLevel = menuLevel;
        }

        public async Task ExecuteAsync()
        {
            Console.WriteLine("Import Card Operations");
        }
    }
}