using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Orders.Models
{
    public class PayOrderModel
    {
        public int OrderId { get; set; }

        public int PaymentMethodId { get; set; }

        public IDictionary<string, string> PaymentParameters { get; set; }
    }
}
