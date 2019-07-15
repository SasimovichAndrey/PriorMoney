using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PriorMoney.ConsoleApp.Model;
using PriorMoney.ConsoleApp.UserInterface.Commands;
using PriorMoney.DataImport.CsvImport;
using PriorMoney.DataImport.CsvImport.Parsers;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using PriorMoney.Storage.Managers;
using PriorMoney.Storage.Mongo.Storage;

namespace PriorMoney.ConsoleApp
{
    public static class AppInitializer
    {
        public static ServiceProvider InitApp(ConsoleAppConfig cfg)
        {
            // init app
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.Default;
            Console.InputEncoding = Encoding.GetEncoding(1251);

            var serviceCollection = new ServiceCollection();

            RegisterStorage(cfg, serviceCollection);

            serviceCollection.AddTransient(typeof(IReportFileChoseStrategy), typeof(DefaultReportFileChoseStrategy));
            serviceCollection.AddTransient(typeof(IModelStringView<CardOperation>), typeof(CardOperationStringView));
            serviceCollection.AddTransient(typeof(ICardOperationParser), typeof(CsvCardOperationParser));
            serviceCollection.AddTransient(typeof(IDateRangeParser), typeof(DateRangeParser));
            serviceCollection.AddTransient(typeof(ICardOperationsLoader), (provider) =>
                new FileSystemOperationsLoader(cfg.OperationsReportsDataFolderPath,
                    provider.GetService<ICardOperationParser>(),
                    provider.GetService<IReportFileChoseStrategy>()));
            serviceCollection.AddTransient(typeof(ImportCardOperationsCommand), typeof(ImportCardOperationsCommand));
            serviceCollection.AddTransient(typeof(ShowOperationsCommand), typeof(ShowOperationsCommand));
            serviceCollection.AddTransient(typeof(ShowOperationsByPeriodCommand), typeof(ShowOperationsByPeriodCommand));
            serviceCollection.AddTransient(typeof(ShowOperationsByCategoryCommand), typeof(ShowOperationsByCategoryCommand));
            serviceCollection.AddTransient(typeof(ManageOperationCategoriesUserInterfaceCommand), typeof(ManageOperationCategoriesUserInterfaceCommand));
            serviceCollection.AddTransient(typeof(SetCardOperationSetCommand), typeof(SetCardOperationSetCommand));
            serviceCollection.AddTransient(typeof(AddCardOperationToCategoryCommand), typeof(AddCardOperationToCategoryCommand));
            serviceCollection.AddTransient(typeof(SearchForOperationsCommand), typeof(SearchForOperationsCommand));
            serviceCollection.AddTransient(typeof(ExitCurrentMenuCommand), typeof(ExitCurrentMenuCommand));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceCollection.AddSingleton(typeof(IServiceProvider), serviceProvider);

            return serviceProvider;
        }

        private static void RegisterStorage(ConsoleAppConfig cfg, IServiceCollection serviceCollection)
        {
            var mongoClient = new MongoClient(cfg.MongoConnectionString);
            var db = mongoClient.GetDatabase(cfg.MongoDbName);

            serviceCollection.AddSingleton(typeof(IMongoDatabase), (provider) => db);

            serviceCollection.AddTransient(typeof(IStorage<CardOperation>), typeof(CardOperationStorage));
            serviceCollection.AddTransient(typeof(IStorage<CardOperationsImport>), typeof(CardOperationsImportStorage));

            serviceCollection.AddTransient(typeof(IDbLogicManager), typeof(DbLogicManager));
        }
    }
}