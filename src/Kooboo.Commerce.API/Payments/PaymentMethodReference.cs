﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Payments
{
    public class PaymentMethodReference
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public PaymentMethodType Type { get; set; }
    }
}
