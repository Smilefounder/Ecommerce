using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Shipping
{
    public interface IShippingMethodQuery : ICommerceQuery<ShippingMethod>
    {
        IShippingMethodQuery ById(int id);

        IShippingMethodQuery ByName(string name);
    }
}
