using PriorMoney.DesktopApp.Model;
using System;
using System.Windows.Input;

namespace PriorMoney.DesktopApp.ViewModel.Commands
{
    public class AddNewCardOperationCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public event EventHandler CanExecuteChanged;

        public AddNewCardOperationCommand(MainWindowViewModel vm)
        {
            _viewModel = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await _viewModel.InitializeNewCardOperationForAdding();
        }
    }
}
