using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class RecommendedItem
    {
        public string ItemId { get; set; }

        public double Weight { get; set; }

        public IDictionary<string, double> Reasons { get; set; }

        public RecommendedItem()
        {
            Reasons = new Dictionary<string, double>();
        }
    }
}