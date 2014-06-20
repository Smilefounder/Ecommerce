using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.ShippingMethods
{
    public class ShippingRateProviderConfigEditorModel
    {
        public int ShippingMethodId { get; set; }

        public object Config { get; set; }
    }
}