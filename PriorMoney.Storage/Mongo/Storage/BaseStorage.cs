using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace PriorMoney.Storage.Mongo.Storage
{
    public class BaseStorage
    {
        private readonly MongoClient _client;

        public BaseStorage(MongoClient client)
        {
            this._client = client;
        }
    }
}
