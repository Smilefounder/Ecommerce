using Kooboo.Commerce.API.Metadata;
using Kooboo.Commerce.API.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Recommendations
{
    [Query("Recommendations")]
    public interface IRecommendationQuery : ICommerceQuery<Product>
    {
        IRecommendationQuery ByProduct(int productId);
    }
}
