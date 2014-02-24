using Kooboo.Commerce.Accounts;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public interface IExtendedQuery<TModel, QModel> where TModel : class, new() where QModel : class, new()
    {
        string Name { get; }
        string Title { get; }
        string Description { get; }

        ExtendedQueryParameter[] Parameters { get; }

        IPagedList<TResult> Query<TResult>(IEnumerable<ExtendedQueryParameter> parameters, ICommerceDatabase db, int pageIndex, int pageSize, Func<QModel, TResult> func);
    }

    public class ExtendedQueryParameter
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object DefaultValue { get; set; }
        public string Description { get; set; }
        public object Value { get; set; }
    }

}
