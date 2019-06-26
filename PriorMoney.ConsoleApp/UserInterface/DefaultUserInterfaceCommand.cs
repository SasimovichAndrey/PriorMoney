using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using PriorMoney.ConsoleApp.UserInterface.Commands;
using System.Linq;
using PriorMoney.Utils;
using PriorMoney.ConsoleApp.UserInterface.CommandDecorators;

namespace PriorMoney.ConsoleApp.UserInterface
{
    public class DefaultUserInterface : CompositeUserInterfaceCommand
    {
        public DefaultUserInterface(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override void InitlizeCommandList(List<ValueTuple<IUserInterfaceCommand, string>> commands)
        {
            var importCardOperationsCommand = _serviceProvider.GetService<ImportCardOperationsCommand>();
            commands.Add((new EmptyLineCommandDecorator(importCardOperationsCommand, false, true), "Импорт операций"));

            var showOperationsMenuCommand = _serviceProvider.GetService<ShowOperationsCommand>();
            commands.Add((new EmptyLineCommandDecorator(showOperationsMenuCommand, false, true), "Показать операции за месяц"));
        }
    }
}