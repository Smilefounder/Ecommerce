using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Accessories
{
    public interface IProductAccessoryService
    {
        IEnumerable<ProductAccessory> GetAccessories(int productId);

        void UpdateAccessories(int productId, IEnumerable<ProductAccessory> accessories);
    }
}