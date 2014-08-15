using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources
{
    static class StringConvert
    {
        public static object ToObject(string value, Type type)
        {
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            if (type.IsValueType && !type.IsPrimitive)
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null)
                {
                    return Convert.ChangeType(value, underlyingType);
                }
            }

            if (type.IsArray)
            {
                var items = value.Split(new[]{","}, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length > 0)
                {
                    var itemType = type.GetElementType();
                    var array = Array.CreateInstance(itemType, items.Length);
                    for (var i = 0; i < items.Length; i++)
                    {
                        array.SetValue(ToObject(items[i], itemType), i);
                    }

                    return array;
                }
                else
                {
                    return null;
                }
            }

            return Convert.ChangeType(value, type);
        }
    }
}