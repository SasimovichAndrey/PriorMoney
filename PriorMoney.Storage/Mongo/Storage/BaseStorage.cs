using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using PriorMoney.Model;

namespace PriorMoney.Storage.Mongo.Storage
{
    public abstract class BaseStorage<T> where T : IHasId
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

        public Task<T> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> Get(Expression<Func<T, bool>> where)
        {
            var collection = await GetCollection().FindAsync(where);

            return collection.ToList();
        }

        public async Task<List<T>> Get(Expression<Func<T, bool>> where, int take, Expression<Func<T, object>> sortBy)
        {
            var collection = GetCollection()
                .Find(where)
                .SortByDescending(sortBy)
                .Limit(take);

            return await collection.ToListAsync();
        }

        public async Task<List<T>> GetAll()
        {
            return await this.Get(item => true);
        }

        public async Task UpdateManyAsync<TField>(List<T> operations, Expression<Func<T, TField>> field)
        {
            var collection = GetCollection();

            var updateOperations = new List<Task>();

            foreach (var op in operations)
            {
                var filter = Builders<T>.Filter.Eq(el => el.Id, op.Id);
                var update = Builders<T>.Update.Set(field, field.Compile().Invoke(op));

                updateOperations.Add(collection.UpdateOneAsync(filter, update));
            }

            await Task.WhenAll(updateOperations);
        }

        protected IMongoCollection<T> GetCollection()
        {
            var collection = _database.GetCollection<T>(GetCollectionName());

            return collection;
        }

        protected abstract string GetCollectionName();
    }
}
