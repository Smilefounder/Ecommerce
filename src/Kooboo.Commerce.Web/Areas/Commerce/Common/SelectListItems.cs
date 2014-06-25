using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web
{
    public static class SelectListItems
    {
        public static IList<SelectListItem> FromEnum<TEnum>()
        {
            Type enumType = typeof (TEnum);
            return FromEnum(enumType);
        }

        public static IList<SelectListItem> FromEnum<TEnum>(TEnum selectedValue)
        {
            Type enumType = typeof (TEnum);
            return FromEnum(enumType, selectedValue.ToString());
        }

        public static IList<SelectListItem> FromEnum(Type enumType)
        {
            return FromEnum(enumType, null);
        }

        public static IList<SelectListItem> FromEnum(Type enumType, object selectedValue)
        {
            IList<SelectListItem> list = new List<SelectListItem>();

            foreach (var field in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                var descAttr = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                    .OfType<DescriptionAttribute>()
                                    .FirstOrDefault();

                var text = descAttr == null ? field.Name : descAttr.Description;
                var value = field.Name;
                var selected = selectedValue != null && (value == selectedValue.ToString());

                list.Add(new SelectListItem {Text = text, Value = value, Selected = selected});
            }

            return list;
        }
    }
}