using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations
{
    public class RecommendationsTabModel
    {
        public int ProductId { get; set; }

        public IList<ProductRecommendation> Recommendations { get; set; }

        public RecommendationsTabModel()
        {
            Recommendations = new List<ProductRecommendation>();
        }
    }
}