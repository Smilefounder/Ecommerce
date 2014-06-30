using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Orders.Pricing.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Orders.Pricing
{
    /// <summary>
    /// Represents a price calculation pipeline in which each stage calculates a portion of the price.
    /// </summary>
    public class PriceCalculator
    {
        private List<IPriceCalculationModule> _modules;

        public PriceCalculator()
        {
            var modules = new List<IPriceCalculationModule>();
            foreach (var type in PriceCalculationModules.Modules)
            {
                modules.Add((IPriceCalculationModule)TypeActivator.CreateInstance(type));
            }

            _modules = modules;
        }

        public PriceCalculator(IEnumerable<IPriceCalculationModule> modules)
        {
            _modules = modules.ToList();
        }

        public void Calculate(PriceCalculationContext context)
        {
            foreach (var module in _modules)
            {
                module.Execute(context);
            }
        }
    }
}
