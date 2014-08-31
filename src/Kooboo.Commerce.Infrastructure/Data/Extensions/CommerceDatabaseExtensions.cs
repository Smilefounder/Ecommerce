using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class CommerceDatabaseExtensions
    {
        public static void Transactional(this ICommerceDatabase database, Action action)
        {
            using (var tx = database.BeginTransaction())
            {
                action();
                tx.Commit();
            }
        }

        public static T Transactional<T>(this ICommerceDatabase database, Func<T> action)
        {
            T result;

            using (var tx = database.BeginTransaction())
            {
                result = action();
                tx.Commit();
            }

            return result;
        }
    }
}
