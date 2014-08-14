﻿using Kooboo.Commerce.Api.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Generic.ApiBased
{
    [DataContract]
    [KnownType(typeof(ShippingMethodDataSource))]
    public class ShippingMethodDataSource : ApiBasedDataSource<ShippingMethod>
    {
        public override string Name
        {
            get { return "ShippingMethods"; }
        }

        protected override Api.Query<ShippingMethod> Query(Api.ICommerceApi api)
        {
            return api.ShippingMethods.Query();
        }
    }
}