using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public class Behavior
    {
        public string UserId { get; set; }

        public string ItemId { get; set; }

        public string Type { get; set; }

        public double Weight { get; set; }

        public DateTime UtcTimestamp { get; set; }

        public Behavior()
        {
            Weight = 1.0d;
            UtcTimestamp = DateTime.UtcNow;
        }
    }
}