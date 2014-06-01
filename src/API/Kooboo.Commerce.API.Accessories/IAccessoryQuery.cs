using Kooboo.Commerce.API.Metadata;
using Kooboo.Commerce.API.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Accessories
{
    [Query("Accessories")]
    public interface IAccessoryQuery : ICommerceQuery<Product>
    {
        IAccessoryQuery ByProduct(int productId);
    }
}
