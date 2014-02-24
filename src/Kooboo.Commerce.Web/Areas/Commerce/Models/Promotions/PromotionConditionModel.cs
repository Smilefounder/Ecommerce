using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions
{
    public class PromotionConditionModel
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool Configurable { get; set; }

        public string CreationUrl { get; set; }
    }
}