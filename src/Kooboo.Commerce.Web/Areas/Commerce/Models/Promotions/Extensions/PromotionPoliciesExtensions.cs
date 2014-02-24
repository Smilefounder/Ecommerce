using Kooboo.Commerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions
{
    public static class PromotionPoliciesExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<IPromotionPolicy> policies)
        {
            return policies.Select(x => new SelectListItem
            {
                Text = x.DisplayName,
                Value = x.Name
            });
        }
    }
}