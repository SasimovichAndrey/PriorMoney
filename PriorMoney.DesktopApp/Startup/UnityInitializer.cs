using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PriorMoney.DataImport.CsvImport;
using PriorMoney.DataImport.CsvImport.Parsers;
using PriorMoney.DataImport.Interface;
using PriorMoney.DesktopApp.Infrastructure;
using PriorMoney.DesktopApp.View;
using PriorMoney.DesktopApp.ViewModel;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using PriorMoney.Storage.Managers;
using PriorMoney.Storage.Mongo.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Injection;

namespace PriorMoney.DesktopApp.Startup
{
    public static class UnityInitializer
    {
        private static readonly UnityContainer _container = new UnityContainer();

        public static void Configure()
        {
            RegisterStorage();

            _container.RegisterType<IDbLogicManager, DbLogicManager>();
            _container.RegisterInstance(AutoMapperConfig.Configure());

            _container.RegisterType<IReportFileChoseStrategy, FileOpenDialogtReportFileChoseStrategy>();
            _container.RegisterType<ICardOperationParser, CsvCardOperationParser>();
            _container.RegisterType<IDateRangeParser, DateRangeParser>();
            _container.RegisterInstance<IConfigurationProvider>(new PriorMoneyDesktopAppConfigProvider());
            _container.RegisterType<ICardOperationsLoader, FileSystemOperationsLoader>();
        }

        public static UnityContainer GetConfiguredContainer()
        {
            return _container;
        }

        private static void RegisterStorage()
        {
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));

            var mongoClient = new MongoClient(ConfigurationManager.AppSettings["MongoConnectionString"]);
            var db = mongoClient.GetDatabase(ConfigurationManager.AppSettings["MongoDbName"]);

            _container.RegisterInstance<IMongoDatabase>(db);

            _container.RegisterType<IStorage<CardOperation>, CardOperationStorage>();
            _container.RegisterType<IStorage<CardOperationsImport>, CardOperationsImportStorage>();
        }
    }
}
