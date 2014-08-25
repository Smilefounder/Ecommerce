using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.CMSIntegration
{
    public class ProductRecommendation
    {
        public Product Product { get; set; }

        public double Weight { get; set; }

        public Product Reason { get; set; }
    }
}