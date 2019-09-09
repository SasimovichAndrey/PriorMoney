using PriorMoney.DataImport.Interface;
using PriorMoney.DesktopApp.View;
using PriorMoney.DesktopApp.ViewModel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace PriorMoney.DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IUnityContainer _unityContainer;

        public MainWindow(MainWindowViewModel vm, IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;

            this.DataContext = vm;

            InitializeComponent();

            

            this.Loaded +=  async (object sender, RoutedEventArgs e) =>
            {
                await vm.LoadData();
                var listView = this.operationsListView;
            };
        }

        private async void StackPanel_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                await ((MainWindowViewModel)this.DataContext).SaveNewOperation();
            }
        }

        private async void OpenImportDialog(object sender, RoutedEventArgs e)
        {
            var importWindow = _unityContainer.Resolve<ImportWindow>();
            importWindow.Top = this.Top + 50;
            importWindow.Left = this.Left + 50;
            importWindow.ShowDialog();

            var vm = (MainWindowViewModel)this.DataContext;
            await vm.LoadData();
        }

        private async void CardOperationView_OperationRemoved(object sender, Model.CardOperationModel cardOperation)
        {
            var vm = (MainWindowViewModel)this.DataContext;
            await vm.RemoveCardOperation(cardOperation);
        }

        private async void cardOperationsListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var vm = (MainWindowViewModel)this.DataContext;
            if (e.VerticalOffset == e.ExtentHeight - e.ViewportHeight && vm.CardOperations.Any() && e.VerticalChange != 0)
            {
                await vm.LoadAdditionalData();
            }
        }

        private async void CardOperationView_OperationSaved(object sender, Model.CardOperationModel cardOperation)
        {
            var vm = (MainWindowViewModel)this.DataContext;
            await vm.SaveCardOperation(cardOperation);
        }
    }
}
