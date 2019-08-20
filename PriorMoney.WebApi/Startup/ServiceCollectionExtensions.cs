using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;
using PriorMoney.Storage.Managers;
using PriorMoney.Storage.Mongo.Storage;

namespace PriorMoney.WebApp
{
    public static class ServiceCollectionExtensions
    {
        public static void AddStorage(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));

            var mongoConnectionUrl = new MongoUrl("mongodb://localhost:27017");
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
            mongoClientSettings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    Debug.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
                    Debug.WriteLine("");
                });

                cb.Subscribe<CommandSucceededEvent>(e =>
                {
                    Debug.WriteLine(e.Reply.ToString());
                    Debug.WriteLine("");
                });
            };
            var mongoClient = new MongoClient(mongoClientSettings);
            var db = mongoClient.GetDatabase("priormoney_test");

            services.AddSingleton(typeof(IMongoDatabase), (provider) => db);

            services.AddTransient(typeof(IStorage<CardOperation>), typeof(CardOperationStorage));
            services.AddTransient(typeof(IStorage<CardOperationsImport>), typeof(CardOperationsImportStorage));

            services.AddTransient(typeof(IDbLogicManager), typeof(DbLogicManager));
        }
    }
}