using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing
{
    public class PriceCalculationModuleCollection : Collection<Type>
    {
        public PriceCalculationModuleCollection() { }

        public PriceCalculationModuleCollection(IEnumerable<Type> moduleTypes)
            : base(moduleTypes.ToList())
        {
        }
    }
}
