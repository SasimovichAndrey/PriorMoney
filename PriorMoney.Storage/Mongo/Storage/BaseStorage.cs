using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using PriorMoney.Model;
using MongoDB.Driver.Linq;
using System.Linq;

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

        public IQueryable<T> GetAllAsQueryable()
        {
            return GetCollection().AsQueryable();
        }

        public async Task<List<TPropElemType>> GetDistinct<TPropElemType>(Expression<Func<T, IEnumerable<TPropElemType>>> collectionProp)
        {
            var collection = GetCollection();

            var distinctAcrossAllPropInstances = await collection.AsQueryable().SelectMany(collectionProp).Distinct().ToListAsync();

            return distinctAcrossAllPropInstances;
        }

        public async Task UpdateManyAsync<TField>(IEnumerable<T> operations, Expression<Func<T, TField>> field)
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

        public async Task UpdateManyAsync(IEnumerable<T> operations)
        {
            var collection = GetCollection();

            foreach (var op in operations)
            {
                FilterDefinition<T> filter = Builders<T>.Filter.Eq(e => e.Id, op.Id);
                await collection.ReplaceOneAsync(filter, op);
            }
        }

        public async Task RemoveById(Guid id)
        {
            var collection = GetCollection();

            await collection.DeleteOneAsync(e => e.Id == id);
        }

        protected IMongoCollection<T> GetCollection()
        {
            var collection = _database.GetCollection<T>(GetCollectionName());

            return collection;
        }

        protected abstract string GetCollectionName();
    }
}
