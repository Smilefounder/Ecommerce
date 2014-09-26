using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Orders.Models
{
    public class SubmitOrderModel
    {
        public bool ClearCart { get; set; }

        public string ReturnUrl { get; set; }
    }
}
