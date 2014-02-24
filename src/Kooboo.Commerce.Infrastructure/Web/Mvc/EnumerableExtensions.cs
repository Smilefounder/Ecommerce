using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Mvc
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> items,
                                                                  Func<T, string> text,
                                                                  Func<T, string> value)
        {
            return ToSelectList(items, text, value, item => false);
        }

        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> items,
                                                                  Func<T, string> text,
                                                                  Func<T, string> value,
                                                                  string selectedValue,
                                                                  string defaultOption = null)
        {
            return ToSelectList(items, text, value, item => value(item) == selectedValue, defaultOption);
        }

        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> items,
                                                                  Func<T, string> text,
                                                                  Func<T, string> value,
                                                                  Func<T, bool> selected,
                                                                  string defaultOption = null)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            if (defaultOption != null)
            {
                selectList.Add(new SelectListItem()
                                   {
                                       Text = defaultOption
                                   });
            }

            selectList.AddRange(items.Select(item => new SelectListItem()
                                                         {
                                                             Text = text(item),
                                                             Value = value(item),
                                                             Selected = selected(item)
                                                         }));

            return selectList;
        }
    }
}