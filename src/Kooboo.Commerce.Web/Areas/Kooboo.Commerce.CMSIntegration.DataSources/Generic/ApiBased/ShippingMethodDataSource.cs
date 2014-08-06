using Kooboo.Commerce.Api.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    public class ShippingMethodDataSource : ApiBasedDataSource
    {
        public ShippingMethodDataSource()
            : base("ShippingMethods", typeof(IShippingMethodQuery), typeof(ShippingMethod))
        {
        }

        protected override object GetQuery(Api.ICommerceApi api)
        {
            return api.ShippingMethods;
        }
    }
}