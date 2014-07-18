using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs.Queries
{
    public abstract class CustomerTabQuery<TResult> : ITabQuery
        where TResult : ICustomerModel
    {
        public abstract string Name { get; }

        public abstract string DisplayName { get; }

        public virtual Type ConfigType
        {
            get
            {
                return null;
            }
        }

        public Type ResultType
        {
            get
            {
                return typeof(TResult);
            }
        }

        public virtual IEnumerable<MvcRoute> ApplyTo
        {
            get
            {
                yield return MvcRoutes.Customers.List();
            }
        }

        public abstract Pagination Execute(QueryContext context);
    }
}
