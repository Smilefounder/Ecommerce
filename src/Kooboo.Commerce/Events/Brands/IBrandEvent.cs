using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    [ActivityVisible("Brand Events")]
    public interface IBrandEvent : IEvent
    {
        int BrandId { get; }
    }
}
