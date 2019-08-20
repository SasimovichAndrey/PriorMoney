using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Threading;

namespace PriorMoney.Utils
{
    public static class QueryableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> queryable)
        {
            if(queryable is IMongoQueryable<T>)
            {
                var mongoQueryable = queryable as IMongoQueryable<T>;
                return await mongoQueryable.ToListAsync(CancellationToken.None);
            }
            else
            {
                throw new ArgumentException("Only IMongoQueryable is supported");
            }
        }
    }
}
