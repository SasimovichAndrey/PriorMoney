﻿using System;
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

            StartUserInterfaceAsync(serviceProvider).Wait();

            serviceProvider.Dispose();
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
