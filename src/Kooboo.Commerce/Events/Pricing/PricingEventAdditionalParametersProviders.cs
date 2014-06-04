using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Pricing
{
    /// <summary>
    /// Because events may be serialized. So it's not convinient to add PricingContext directly to the event class.
    /// So we use this parameter provider to access the additional parameters from the not-directly referenced PricingContext class.
    /// </summary>
    [Dependency(typeof(IParameterProvider), Key = "PricingEventAdditionalParametersProviders")]
    public class PricingEventAdditionalParametersProviders : IParameterProvider
    {
        public IEnumerable<ConditionParameter> GetParameters(Type dataContextType)
        {
            if (!typeof(IPricingEvent).IsAssignableFrom(dataContextType))
            {
                return Enumerable.Empty<ConditionParameter>();
            }

            return new DeclaringParameterProvider().GetParameters(typeof(PricingContext), _ => PricingContext.GetCurrent());
        }
    }
}
