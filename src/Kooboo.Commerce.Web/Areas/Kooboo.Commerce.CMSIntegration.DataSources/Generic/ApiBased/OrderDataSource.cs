using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    public class OrderDataSource : ApiBasedDataSource
    {
        public OrderDataSource()
            : base("Orders", typeof(IOrderQuery), typeof(Order))
        {
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Orders;
        }
    }
}