using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Products
{
    public interface IProductAPI : IProductQuery, IProductAccess
    {
        IProductQuery Query();
        IProductAccess Access();
    }
}
