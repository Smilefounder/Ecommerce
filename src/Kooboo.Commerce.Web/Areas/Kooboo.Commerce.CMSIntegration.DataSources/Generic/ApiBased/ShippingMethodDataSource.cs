using Kooboo.Commerce.Api.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(ShippingMethodDataSource))]
    public class ShippingMethodDataSource : ApiBasedDataSource
    {
        public override string Name
        {
            get { return "ShippingMethods"; }
        }

        protected override Type QueryType
        {
            get { return typeof(IShippingMethodQuery); }
        }

        protected override Type ItemType
        {
            get { return typeof(ShippingMethod); }
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.ShippingMethods;
        }
    }
}