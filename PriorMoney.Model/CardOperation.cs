using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace PriorMoney.Model
{
    public class CardOperation : IHasId
    {
        [BsonId]
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string UserDefinedName { get; set; }

        public string OriginalName { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }

        public Guid ImportId { get; set; }

        public HashSet<string> Categories { get; set; } = new HashSet<string>();

        public override string ToString()
        {
            return $"{DateTime} {UserDefinedName} {Amount} {Currency}";
        }

        public CardOperation Clone()
        {
            return new CardOperation
            {
                Id = Id,
                ImportId = ImportId,
                UserDefinedName = UserDefinedName,
                DateTime = DateTime,
                Amount = Amount,
                Currency = Currency,
                OriginalName = OriginalName,
                Categories = new HashSet<string>(Categories)
            };
        }
    }
}
