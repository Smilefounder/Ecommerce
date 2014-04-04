using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations
{
    public interface IProductRecommendationService
    {
        IEnumerable<ProductRecommendation> GetRecommendations(int productId);

        void SaveRecommendations(int productId, IEnumerable<ProductRecommendation> recommendations);
    }
}