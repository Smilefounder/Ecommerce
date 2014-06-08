using Kooboo.Commerce.API.Metadata;
using Kooboo.Commerce.API.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.API.BestSellers
{
    // TODO: Might need refactoring, see notes for BestSellersQuery
    [Query("BestSellers")]
    public interface IBestSellersQuery : ICommerceQuery<Product>
    {
    }
}
