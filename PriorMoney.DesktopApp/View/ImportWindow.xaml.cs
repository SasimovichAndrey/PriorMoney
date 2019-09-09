using PriorMoney.DesktopApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PriorMoney.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {
        public ImportWindow(ImportWindowViewModel vm)
        {
            this.DataContext = vm;

            InitializeComponent();

            this.Loaded += async (object sender, RoutedEventArgs e) =>
            {
                await vm.LoadData();
            };
        }

        private async void CardOperationView_OperationRemoved(object sender, Model.CardOperationModel cardOperation)
        {
            var vm = (ImportWindowViewModel)this.DataContext;
            await vm.RemoveCardOperation(cardOperation);
        }
    }
}
