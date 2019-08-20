using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PriorMoney.ConsoleApp.Model;
using PriorMoney.ConsoleApp.UserInterface;
using PriorMoney.ConsoleApp.UserInterface.Commands;
using PriorMoney.DataImport.CsvImport;
using PriorMoney.DataImport.CsvImport.Parsers;
using PriorMoney.DataImport.Interface;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using PriorMoney.Storage.Mongo.Storage;

namespace PriorMoney.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var cfg = ReadConfig();

            var serviceProvider = AppInitializer.InitApp(cfg);

#if DEBUG
            InitTestData(serviceProvider);
#endif
            StartUserInterfaceAsync(serviceProvider).Wait();

            serviceProvider.Dispose();
        }

        private static void InitTestData(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<IMongoDatabase>();

            var cardOperationsCollection = db.GetCollection<CardOperation>("CardOperations");
            cardOperationsCollection.DeleteMany(FilterDefinition<CardOperation>.Empty);

            var referenceDateTime = DateTime.Now;
            var cardOperations = new List<CardOperation>()
            {
                new CardOperation{
                    OriginalName = "AZS 15 GH653",
                    UserDefinedName = "Топливо",
                    Amount = -30.10m,
                    Currency = Currency.BYN,
                    DateTime = referenceDateTime.AddDays(-20),
                    Categories = new HashSet<string>(){"Машина", "Транспорт"}
                },
                new CardOperation{
                    OriginalName = "LIMOZH Shop HG211",
                    UserDefinedName = "Продукты",
                    Amount = -12.34m,
                    Currency = Currency.BYN,
                    DateTime = referenceDateTime.AddDays(-19),
                    Categories = new HashSet<string>(){"Продукты"}
                },
                new CardOperation{
                    OriginalName = "KIOSK Tvister 326363",
                    UserDefinedName = "Бургеры",
                    Amount = -12.34m,
                    Currency = Currency.BYN,
                    DateTime = referenceDateTime.AddDays(-19),
                    Categories = new HashSet<string>(){"Бургеры", "Еда"}
                },
                new CardOperation{
                    OriginalName = "Stolovka",
                    UserDefinedName = "Столовая",
                    Amount = -22.34m,
                    Currency = Currency.BYN,
                    DateTime = referenceDateTime.AddDays(-19)
                },
                new CardOperation{
                    OriginalName = "Stolovka",
                    UserDefinedName = "Столовая",
                    Amount = -22.34m,
                    Currency = Currency.BYN,
                    DateTime = referenceDateTime.AddDays(-19),
                    Categories = new HashSet<string>()
        }
            };

            cardOperations.ForEach(op =>
            {
                if (op.Categories != null) op.Categories = op.Categories.Select(c => c.ToUpper().Trim()).ToHashSet();
            });

            cardOperationsCollection.InsertMany(cardOperations);
        }

        private async static Task StartUserInterfaceAsync(IServiceProvider serviceProvider)
        {
            IUserInterfaceCommand initialUserInterfaceCommand = new DefaultUserInterface(serviceProvider);
            await initialUserInterfaceCommand.ExecuteAsync();
        }



        private static ConsoleAppConfig ReadConfig()
        {
#if DEBUG
            var cfgFileName = "appsettings.json";
#else
            var cfgFileName = "appsettings.Release.json";
#endif
            IConfiguration jsonConfig = new ConfigurationBuilder()
                .AddJsonFile(cfgFileName, true, true)
                .Build();

            var cfg = new ConsoleAppConfig();

            cfg.OperationsReportsDataFolderPath = jsonConfig["OperationsReportsDataFolderPath"];
            cfg.MongoConnectionString = jsonConfig["MongoConnectionString"];
            cfg.MongoDbName = jsonConfig["MongoDbName"];

            return cfg;
        }
    }
}
