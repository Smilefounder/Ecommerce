using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models
{
    public class AddItemModel : SubmissionModel
    {
        public int ProductPriceId { get; set; }

        public int Quantity { get; set; }
    }
}
