using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public static class CommerceDatabaseExtensions
    {
        public static void WithTransaction(this ICommerceDatabase db, Action action)
        {
            using (var tx = db.BeginTransaction())
            {
                action();
                tx.Commit();
            }
        }
    }
}
