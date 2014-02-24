using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Mvc
{
    public static class SelectListExtensions
    {
        public static IEnumerable<SelectListItemEx> ToSelectListEx(this IEnumerable<SelectListItem> items)
        {
            foreach (var item in items)
            {
                yield return new SelectListItemEx { Text = item.Text, Value = item.Value, Selected = item.Selected };
            }
        }
    }
}
