using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Pricing
{
    [Category("Pricing", Order = 900)]
    public interface IPricingEvent : IBusinessEvent
    {
    }
}
