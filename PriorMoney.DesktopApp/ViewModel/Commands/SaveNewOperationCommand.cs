using PriorMoney.DesktopApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PriorMoney.DesktopApp.ViewModel.Commands
{
    public class SaveNewOperationCommand : ICommand
    {
        private MainWindowViewModel _viewModel;

        public event EventHandler CanExecuteChanged;

        public SaveNewOperationCommand(MainWindowViewModel vm)
        {
            _viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter != null)
            {
                var cardOPerationModel = parameter as CardOperationModel;
                return cardOPerationModel.IsModelReadyForSaving();
            }

            return false;
        }

        public async void Execute(object parameter)
        {
            await _viewModel.SaveNewOperation();
        }

        public void RaiseOperationModelChanged(object sender, PropertyChangedEventArgs e)
        {
            if(CanExecuteChanged != null)
                CanExecuteChanged(this, e);
        }
    }
}
