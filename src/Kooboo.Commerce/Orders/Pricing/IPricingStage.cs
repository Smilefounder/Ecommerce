using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing
{
    /// <summary>
    /// Represents a price calculation stage in the price calculation pipeline.
    /// </summary>
    public interface IPricingStage
    {
        string Name { get; }

        void Execute(PricingContext context);
    }
}
