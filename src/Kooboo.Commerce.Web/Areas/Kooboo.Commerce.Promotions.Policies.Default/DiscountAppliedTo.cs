using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Promotions.Policies.Default
{
    public enum DiscountAppliedTo
    {
        [Description("Products matched by promotion conditions")]
        MatchedProducts = 0,

        [Description("Order subtotal")]
        OrderSubtotal = 1,

        [Description("Order shipping")]
        OrderShipping = 2
    }
}