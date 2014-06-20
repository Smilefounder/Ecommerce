using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public class ActivityParameters
    {
        private Dictionary<string, string> _values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public string GetValue(string key)
        {
            string value;
            if (_values.TryGetValue(key, out value))
            {
                return value;
            }

            return null;
        }

        public T GetValue<T>(string key, T defaultValue = default(T))
        {
            var value = GetValue(key);
            if (value == null)
            {
                return defaultValue;
            }

            if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), value);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public void SetValue(string key, object value)
        {
            string strValue = null;

            if (value != null)
            {
                strValue = value as string;
                if (strValue == null)
                {
                    strValue = value.ToString();
                }
            }

            if (_values.ContainsKey(key))
            {
                _values[key] = strValue;
            }
            else
            {
                _values.Add(key, strValue);
            }
        }

        public IDictionary<string, string> GetValues()
        {
            return new Dictionary<string, string>(_values);
        }

        public void SetValues(IDictionary<string, string> values)
        {
            foreach (var each in values)
            {
                SetValue(each.Key, each.Value);
            }
        }

        public static T Create<T>(IDictionary<string, string> values = null)
            where T : ActivityParameters
        {
            return Create(typeof(T), values) as T;
        }

        public static ActivityParameters Create(Type parameterType, IDictionary<string, string> values = null)
        {
            var instance = Activator.CreateInstance(parameterType) as ActivityParameters;
            if (instance == null)
                throw new InvalidOperationException("Activity parameters type must inherit from " + parameterType + ".");

            if (values != null)
            {
                instance.SetValues(values);
            }

            return instance;
        }
    }
}
