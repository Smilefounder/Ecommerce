﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models
{
    public class ChangeShippingAddressModel
    {
        public int ShippingAddressId { get; set; }

        public string SuccessUrl { get; set; }
    }
}
