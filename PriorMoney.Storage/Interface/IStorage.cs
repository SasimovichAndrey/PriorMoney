using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PriorMoney.Storage.Interface
{
    public interface IStorage<T>
    {
        void Add(T entity);

        void AddMany(IEnumerable<T> entities);

        T Get(Guid id);
        List<T> Get(Expression<Func<T, bool>> where);
    }
}
