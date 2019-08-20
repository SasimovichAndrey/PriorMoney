using PriorMoney.DesktopApp.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace PriorMoney.DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel vm)
        {
            this.DataContext = vm;

            InitializeComponent();

            this.Loaded +=  async (object sender, RoutedEventArgs e) => await vm.LoadData();
        }

        private async void StackPanel_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                await ((MainWindowViewModel)this.DataContext).SaveNewOperation();
            }
        }
    }
}
