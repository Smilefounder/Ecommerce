using Kooboo.Commerce.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Shipping.ByWeight.Models
{
    public class ByWeightShippingRulesModel
    {
        public int ShippingMethodId { get; set; }

        public IList<ByWeightShippingRuleModel> Rules { get; set; }

        public IList<SelectListItem> AvailablePriceUnits { get; set; }

        public ByWeightShippingRulesModel()
        {
            Rules = new List<ByWeightShippingRuleModel>();
            AvailablePriceUnits = SelectListItems.FromEnum<ShippingPriceUnit>();
        }
    }
}