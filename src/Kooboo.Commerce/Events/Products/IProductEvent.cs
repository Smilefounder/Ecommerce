﻿using Kooboo.Commerce.Products;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Activities;
using System.ComponentModel;

namespace Kooboo.Commerce.Events.Products
{
    public interface IProductEvent : IEvent
    {
        int ProductId { get; }
    }
}
