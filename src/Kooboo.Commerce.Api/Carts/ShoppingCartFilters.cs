using Kooboo.Commerce.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Carts
{
    public static class ShoppingCartFilters
    {
        public static readonly FilterDescription ById = new FilterDescription("ById", new ParameterDescription("Id", typeof(Int32)));

        public static readonly FilterDescription BySessionId = new FilterDescription("BySessionId", new ParameterDescription("SessionId", typeof(String)));

        public static readonly FilterDescription ByAccountId = new FilterDescription("ByAccountId", new ParameterDescription("AccountId", typeof(String)));

        public static readonly FilterDescription ByCurrentCustomer = new FilterDescription("ByCurrentCustomer");
    }
}
