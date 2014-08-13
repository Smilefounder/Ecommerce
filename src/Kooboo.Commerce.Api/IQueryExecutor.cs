using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    public interface IQueryExecutor<T>
        where T : class
    {
        int Count(Query<T> query);

        T FirstOrDefault(Query<T> query);

        List<T> ToList(Query<T> query);
    }
}
