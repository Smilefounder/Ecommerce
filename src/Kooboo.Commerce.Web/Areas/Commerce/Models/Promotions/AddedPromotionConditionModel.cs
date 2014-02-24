using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions
{
    public class AddedPromotionConditionModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool Configurable { get; set; }

        public string EditorUrl { get; set; }
    }
}