using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriorMoney.Storage.Interface
{
    public interface IStorage<T>
    {
        Task Add(T entity);

        Task AddMany(IEnumerable<T> entities);

        Task<T> Get(Guid id);
        Task<List<T>> Get(Expression<Func<T, bool>> where);
    }
}
