using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(OrderDataSource))]
    public class OrderDataSource : ApiBasedDataSource
    {
        public override string Name
        {
            get { return "Orders"; }
        }

        protected override Type QueryType
        {
            get { return typeof(IOrderQuery); }
        }

        protected override Type ItemType
        {
            get { return typeof(Order); }
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.Orders;
        }
    }
}