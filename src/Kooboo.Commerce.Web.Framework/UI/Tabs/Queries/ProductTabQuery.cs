using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs.Queries
{
    public abstract class ProductTabQuery<TResult> : ITabQuery
        where TResult : IProductModel
    {
        public abstract string Name { get; }

        public virtual string DisplayName
        {
            get
            {
                return Name;
            }
        }

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
                yield return MvcRoutes.Products.List();
            }
        }

        public abstract Pagination Execute(QueryContext context);
    }
}
