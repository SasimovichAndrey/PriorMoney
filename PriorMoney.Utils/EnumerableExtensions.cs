using System.Collections.Generic;
using System.Linq;

namespace PriorMoney.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SelectUnique<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.GroupBy(el => el).Select(g => g.First());
        }
    }
}