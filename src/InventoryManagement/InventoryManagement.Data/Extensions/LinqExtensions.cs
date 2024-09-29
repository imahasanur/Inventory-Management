using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Data.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> enumerable, int page, int limit)
        {
            return page > 0 && limit > 0
                ? enumerable.Skip((page - 1) * limit).Take(limit)
                : enumerable;
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int page, int limit)
        {
            return page > 0 && limit > 0
                ? queryable.Skip((page - 1) * limit).Take(limit)
                : queryable;
        }
    }
}
