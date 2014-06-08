using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.CMSIntegration.Plugins.Carts.Models
{
    public class ChangeShippingAddressModel : SubmissionModel
    {
        public int NewShippingAddressId { get; set; }
    }
}
