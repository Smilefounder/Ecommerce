using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Impl
{
    [DataContract]
    [KnownType(typeof(OrderDataSource))]
    public class OrderDataSource : ApiQueryBasedDataSource<Order>
    {
        public override string Name
        {
            get { return "Orders"; }
        }

        protected override Api.Query<Order> Query(Api.ICommerceApi api)
        {
            return api.Orders.Query();
        }
    }
}