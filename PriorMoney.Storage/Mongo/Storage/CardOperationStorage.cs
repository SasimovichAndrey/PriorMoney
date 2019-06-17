using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;

namespace PriorMoney.Storage.Mongo.Storage
{
    public class CardOperationStorage : BaseStorage, IStorage<CardOperation>
    {
        public CardOperationStorage(MongoClient client) : base(client)
        {
        }

        public void Add(CardOperation entity)
        {
            throw new NotImplementedException();
        }

        public CardOperation Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<CardOperation> Get(Expression<Func<CardOperation, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
