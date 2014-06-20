using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions
{
    public class PromotionPolicyConfigEditorModel
    {
        public int PromotionId { get; set; }

        public object Config { get; set; }
    }
}