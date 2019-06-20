using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;

namespace PriorMoney.Storage.Mongo.Storage
{
    public class CardOperationStorage : BaseStorage<CardOperation>, IStorage<CardOperation>
    {
        public CardOperationStorage(IMongoDatabase database) : base(database)
        {
        }
        
        protected override string GetCollectionName()
        {
            return "CardOperations";
        }
    }
}
