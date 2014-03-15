using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce
{
    public static class TypeExtensions
    {
        public static string GetDescription(this Type type)
        {
            var attr = type.GetCustomAttributes(typeof(DescriptionAttribute), false)
                           .OfType<DescriptionAttribute>()
                           .FirstOrDefault();

            return attr == null ? null : attr.Description;
        }
    }
}
