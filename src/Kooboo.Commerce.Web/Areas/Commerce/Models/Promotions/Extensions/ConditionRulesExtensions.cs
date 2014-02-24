using Kooboo.Commerce.Promotions;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions
{
    public static class ConditionRulesExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<IPromotionCondition> rules)
        {
            return rules.Select(x => new SelectListItem
            {
                Text = (x.GetType().GetDescription() ?? x.Name).Localize(),
                Value = x.Name
            });
        }
    }
}