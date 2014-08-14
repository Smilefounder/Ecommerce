using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Api.Shipping;
using Kooboo.Commerce.Shipping.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Local.Shipping
{
    public class ShippingMethodApi : IShippingMethodApi
    {
        private LocalApiContext _context;

        public ShippingMethodApi(LocalApiContext context)
        {
            _context = context;
        }

        public Query<ShippingMethod> Query()
        {
            return new Query<ShippingMethod>(new ShippingMethodQueryExecutor(_context));
        }
    }
}
