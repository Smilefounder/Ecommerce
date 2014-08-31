using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models
{
    public class AddCartItemModel
    {
        public int ProductVariantId { get; set; }

        public int Quantity { get; set; }

        public string SuccessUrl { get; set; }
    }
}
