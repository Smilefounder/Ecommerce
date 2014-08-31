using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models
{
    public class ChangeCartItemQuantityModel
    {
        public int ItemId { get; set; }

        public int NewQuantity { get; set; }

        public string SuccessUrl { get; set; }
    }
}
