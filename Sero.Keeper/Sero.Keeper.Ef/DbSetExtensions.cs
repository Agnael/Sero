using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Keeper.Ef
{
    public static class DbSetExtensions
    {
        /// <summary>
        ///     Filters out all entities marked as deleted.
        /// </summary>
        public static IQueryable<T> NonDeletedOnly<T>(this IQueryable<T> query)
            where T : Entity
        {
            //query = query.Where(x => !x.IsDeleted);
            return query;
        }
    }
}
