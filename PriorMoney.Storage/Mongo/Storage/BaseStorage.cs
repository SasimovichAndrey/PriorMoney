using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using MongoDB.Driver;

namespace PriorMoney.Storage.Mongo.Storage
{
    public abstract class BaseStorage<T>
    {
        protected readonly IMongoDatabase _database;

        public BaseStorage(IMongoDatabase database)
        {
            this._database = database;
        }

        public void Add(T entity)
        {
            var collection = GetCollection();

            collection.InsertOne(entity);
        }

        public void AddMany(IEnumerable<T> entities)
        {
            var collection = GetCollection();

            collection.InsertMany(entities);
        }

        public T Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<T> Get(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }
        
        protected IMongoCollection<T> GetCollection()
        {
            var collection = _database.GetCollection<T>(GetCollectionName());

            return collection;
        }

        protected abstract string GetCollectionName();
    }
}
