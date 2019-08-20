using PriorMoney.DesktopApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PriorMoney.DesktopApp.ViewModel.Commands
{
    public class RemoveOperationCommand : ICommand
    {
        private MainWindowViewModel _vm;

        public event EventHandler CanExecuteChanged;

        public RemoveOperationCommand(MainWindowViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await _vm.RemoveCardOperation(parameter as CardOperationModel);
        }
    }
}
