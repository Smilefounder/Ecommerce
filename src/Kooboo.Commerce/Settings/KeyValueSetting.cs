using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Settings
{
    public class KeyValueSetting
    {
        [Key]
        public string Category { get; protected set; }

        [Key]
        public string Key { get; protected set; }

        public string Value { get; set; }

        protected KeyValueSetting() { }

        public KeyValueSetting(string key, string category)
        {
            Key = key;
            Category = category;
        }

        public T GetValue<T>()
        {
            var resultType = typeof(T);

            if (resultType == typeof(string))
            {
                return (T)(object)Value;
            }

            if (String.IsNullOrEmpty(Value))
            {
                return default(T);
            }

            if (resultType.IsValueType)
            {
                return (T)Convert.ChangeType(Value, resultType);
            }

            return JsonConvert.DeserializeObject<T>(Value);
        }

        public void SetValue(object value)
        {
            string strValue = null;

            if (value != null)
            {
                var valueType = value.GetType();
                if (valueType.IsValueType || valueType == typeof(String))
                {
                    strValue = value.ToString();
                }
                else
                {
                    strValue = JsonConvert.SerializeObject(value);
                }
            }

            Value = strValue;
        }
    }
}
