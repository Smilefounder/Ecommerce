using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Html
{
    public static class SelectListExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                yield return new SelectListItem
                {
                    Text = item.ToString(),
                    Value = item.ToString()
                };
            }
        }
    }
}