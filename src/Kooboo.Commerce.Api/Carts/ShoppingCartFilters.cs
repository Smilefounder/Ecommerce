using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Carts
{
    public static class ShoppingCartFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new Int32ParameterDescription("Id", true));

        public static readonly FilterDescription BySessionId = new FilterDescription("BySessionId", new StringParameterDescription("SessionId", true));

        public static readonly FilterDescription ByCustomerEmail = new FilterDescription("ByCustomerEmail", new StringParameterDescription("CustomerEmail", true));
    }
}
