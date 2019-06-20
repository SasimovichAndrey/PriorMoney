using System;
using MongoDB.Bson.Serialization.Attributes;

namespace PriorMoney.Model
{
    public class CardOperationsImport
    {
        [BsonId]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
    }
}