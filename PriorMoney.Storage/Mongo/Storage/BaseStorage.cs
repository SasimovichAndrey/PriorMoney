using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

        public async Task Add(T entity)
        {
            var collection = GetCollection();

            await collection.InsertOneAsync(entity);
        }

        public async Task AddMany(IEnumerable<T> entities)
        {
            var collection = GetCollection();

            await collection.InsertManyAsync(entities);
        }

        public async Task<T> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> Get(Expression<Func<T, bool>> where)
        {
            var collection = await GetCollection().FindAsync(where);

            return collection.ToList();
        }

        protected IMongoCollection<T> GetCollection()
        {
            var collection = _database.GetCollection<T>(GetCollectionName());

            return collection;
        }

        protected abstract string GetCollectionName();
    }
}
