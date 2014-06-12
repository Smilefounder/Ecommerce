using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.Checkout
{
    [Category("Checkout", Order = 1000)]
    public interface ICheckoutEvent : IBusinessEvent
    {
    }
}
