using Kooboo.Commerce.API.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Accessories
{
    public interface IAccessoryAPI
    {
        Product[] ForProduct(int productId);
    }
}
