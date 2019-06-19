using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriorMoney.ConsoleApp.Model;
using PriorMoney.ConsoleApp.UserInterface;
using PriorMoney.ConsoleApp.UserInterface.Commands;
using PriorMoney.DataImport.CsvImport;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;

namespace PriorMoney.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var cfg = ReadConfig();

            var serviceProvider = InitApp(cfg);

            StartUserInterfaceAsync(serviceProvider).Wait();

            serviceProvider.Dispose();
        }

        private async static Task StartUserInterfaceAsync(IServiceProvider serviceProvider)
        {
            IConsoleAppUserInterface userInterface = new DefaultUserInterface(serviceProvider);
            await userInterface.StartAsync();
        }

        private static ServiceProvider InitApp(ConsoleAppConfig cfg)
        {
            // init app
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient(typeof(IReportFileChoseStrategy), typeof(DefaultReportFileChoseStrategy));
            serviceCollection.AddTransient(typeof(IModelStringView<CardOperation>), typeof(CardOperationStringView));
            serviceCollection.AddTransient(typeof(ICardOperationParser), typeof(CsvCardOperationParser));
            serviceCollection.AddTransient(typeof(ICardOperationsLoader), (provider) =>
                new FileSystemOperationsLoader(cfg.OperationsReportsDataFolderPath,
                    provider.GetService<ICardOperationParser>(),
                    provider.GetService<IReportFileChoseStrategy>()));
            serviceCollection.AddTransient(typeof(ImportCardOperationsCommand), typeof(ImportCardOperationsCommand));
            serviceCollection.AddTransient(typeof(ExitCurrentMenuCommand), typeof(ExitCurrentMenuCommand));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }

        private static ConsoleAppConfig ReadConfig()
        {
            IConfiguration jsonConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var cfg = new ConsoleAppConfig();

            cfg.OperationsReportsDataFolderPath = jsonConfig["OperationsReportsDataFolderPath"];

            return cfg;
        }
    }
}
