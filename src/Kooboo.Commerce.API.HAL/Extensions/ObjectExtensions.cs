using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.HAL
{
    static class ObjectExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            if (obj != null && obj is IDictionary<string, object>)
            {
                return obj as IDictionary<string, object>;
            }

            var dictionary = new Dictionary<string, object>();
            if (obj != null)
            {
                var properties = TypeDescriptor.GetProperties(obj);
                foreach (PropertyDescriptor propertyDescriptor in properties)
                {
                    var value = propertyDescriptor.GetValue(obj);
                    dictionary.Add(propertyDescriptor.Name, value);
                }
            }

            return dictionary;
        }
    }
}
