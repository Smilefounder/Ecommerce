﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Events.ShippingMethods
{
    [Event(Order = 200)]
    public class ShippingMethodUpdated : BusinessEvent, IShippingMethodEvent
    {
    }
}
