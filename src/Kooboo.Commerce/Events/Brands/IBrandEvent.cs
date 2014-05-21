using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Brands
{
    [Category("Brand Events")]
    public interface IBrandEvent : IDomainEvent
    {
        int BrandId { get; }
    }
}
