using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PriorMoney.DesktopApp.ViewModel.Commands
{
    public class CommandHandler : ICommand
    {
        private Func<object, bool> _canExecuteDelegate;
        private Func<object, Task> _executeDelegate;

        public event EventHandler CanExecuteChanged;

        public CommandHandler(Func<object, bool> canExecuteDelegate, Func<object, Task> executeDelegate)
        {
            _canExecuteDelegate = canExecuteDelegate;
            _executeDelegate = executeDelegate;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteDelegate(parameter);
        }

        public async void Execute(object parameter)
        {
            await _executeDelegate(parameter);
        }

        public void FireCanExecuteChanged()
        {
            if(this.CanExecuteChanged != null)
            {
                CanExecuteChanged.Invoke(this, null);
            }
        }
    }
}
