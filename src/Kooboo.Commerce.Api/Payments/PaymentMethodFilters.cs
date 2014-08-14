using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Payments
{
    public static class PaymentMethodFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new Int32ParameterDescription("Id", true));

        public static readonly FilterDescription ByName = new FilterDescription("ByName", new StringParameterDescription("Name", true));

        public static readonly FilterDescription ByUserKey = new FilterDescription("ByUserKey", new StringParameterDescription("UserKey", true));
    }
}
