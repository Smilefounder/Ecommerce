using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Orders
{
    public class PayOrderResult
    {
        public string PaymentStatus { get; set; }

        public string Message { get; set; }

        public string RedirectUrl { get; set; }
    }
}
