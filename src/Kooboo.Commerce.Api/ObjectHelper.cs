using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api
{
    static class ObjectHelper
    {
        public static Dictionary<string, object> AnonymousToDictionary(object obj, StringComparer comparer)
        {
            var dic = new Dictionary<string, object>(comparer);

            if (obj != null)
            {
                var type = obj.GetType();
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(type))
                {
                    var propValue = prop.GetValue(obj);
                    dic.Add(prop.Name, propValue);
                }
            }

            return dic;
        }
    }
}
