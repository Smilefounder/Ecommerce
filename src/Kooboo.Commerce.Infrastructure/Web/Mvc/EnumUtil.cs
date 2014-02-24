using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Mvc
{
    public static class EnumUtil
    {
        public static IList<SelectListItem> ToSelectList<TEnum>()
        {
            Type enumType = typeof (TEnum);
            return ToSelectList(enumType);
        }

        public static IList<SelectListItem> ToSelectList<TEnum>(TEnum selectedValue)
        {
            Type enumType = typeof (TEnum);
            return ToSelectList(enumType, selectedValue.ToString());
        }

        public static IList<SelectListItem> ToSelectList(Type enumType)
        {
            return ToSelectList(enumType, null);
        }

        public static IList<SelectListItem> ToSelectList(Type enumType, string selectedValue)
        {
            IList<SelectListItem> list = new List<SelectListItem>();

            foreach (var field in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                var descAttr = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                    .OfType<DescriptionAttribute>()
                                    .FirstOrDefault();

                string text = descAttr == null ? field.Name : descAttr.Description;
                string value = field.Name;
                bool selected = (value == selectedValue);

                list.Add(new SelectListItem {Text = text, Value = value, Selected = selected});
            }

            return list;
        }
    }
}