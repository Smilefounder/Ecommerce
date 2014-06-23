using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models
{
    public class RemoveItemModel
    {
        public int ItemId { get; set; }

        public string SuccessUrl { get; set; }
    }
}
