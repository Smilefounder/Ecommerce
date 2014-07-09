using Kooboo.Commerce.Data;
using Kooboo.Commerce.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public interface IExtendedQuery
    {
        string Name { get; }
        string Title { get; }
        string Description { get; }

        ExtendedQueryParameter[] Parameters { get; }
    }
    public interface IExtendedQuery<QModel> : IExtendedQuery where QModel : class, new()
    {
        IPagedList<QModel> Query(IEnumerable<ExtendedQueryParameter> parameters, ICommerceDatabase db, int pageIndex, int pageSize);
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
