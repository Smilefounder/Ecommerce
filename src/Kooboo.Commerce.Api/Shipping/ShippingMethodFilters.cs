using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Shipping
{
    public static class ShippingMethodFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new Int32ParameterDescription("Id"));

        public static readonly FilterDescription ByName = new FilterDescription("ByName", new StringParameterDescription("Name"));
    }
}
