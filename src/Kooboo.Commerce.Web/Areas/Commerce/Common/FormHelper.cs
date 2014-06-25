using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web
{
    public class FormHelper
    {
        public static IList<T> BindToModels<T>(NameValueCollection form, string prefix = "")
            where T : class, new()
        {
            List<T> models = new List<T>();
            var props = typeof(T).GetProperties();
            
            foreach(string k in form.Keys)
            {
                var prop = props.FirstOrDefault(o => (prefix + o.Name) == k);
                if (prop != null)
                {
                    var values = form.GetValues(k);
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (models.Count <= i) { models.Add(new T()); }
                        object val = values[i].ConvertTo(prop.PropertyType, null);
                        prop.SetValue(models[i], val, null);
                    }
                }
            }

            return models;
        }

        public static T BindToModel<T>(NameValueCollection form, string prefix = "")
            where T : class, new()
        {
            T model = new T();
            var props = typeof(T).GetProperties();

            foreach (string k in form.Keys)
            {
                var prop = props.FirstOrDefault(o => (prefix + o.Name) == k);
                if (prop != null)
                {
                    var value = form[k];
                    object val = value.ConvertTo(prop.PropertyType, null);
                    prop.SetValue(model, val, null);
                }
            }

            return model;
        }

        public static ViewDataDictionary AppendOrReplace(ViewDataDictionary dic, string key, object value)
        {
            if (dic.ContainsKey(key))
                dic[key] = value;
            else
                dic.Add(key, value);
            return dic;
        }
    }
}
