using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public static class BehaviorTypes
    {
        public static readonly string View = "View";

        public static readonly string AddToCart = "AddToCart";

        public static readonly string Purchase = "Purchase";

        public static IEnumerable<string> All()
        {
            return new[] { View, AddToCart, Purchase };
        }
    }
}