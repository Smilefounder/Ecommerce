using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Newtonsoft.Json;

namespace Kooboo.Commerce.Settings.Services
{

    [Dependency(typeof(ISettingService))]
    public class SettingService : ISettingService
    {
        private readonly IRepository<KeyValueSetting> _repository;

        public SettingService(IRepository<KeyValueSetting> repository)
        {
            _repository = repository;
        }

        public const string DefaultCategory = "__default";

        public void Set(string key, object value, string category)
        {
            Require.NotNullOrEmpty(key, "key");

            if (String.IsNullOrEmpty(category))
            {
                category = DefaultCategory;
            }

            string strValue = null;

            if (value != null)
            {
                var valueType = value.GetType();
                if (valueType.IsValueType || valueType == typeof(String))
                {
                    strValue = valueType.ToString();
                }
                else
                {
                    strValue = JsonConvert.SerializeObject(value);
                }
            }

            var entry = _repository.Get(key, category);
            if (entry != null)
            {
                entry.Value = strValue;
            }
            else
            {
                entry = new KeyValueSetting(key, category)
                {
                    Value = strValue
                };
                _repository.Insert(entry);
            }
        }

        public string Get(string key, string category = null)
        {
            if (String.IsNullOrEmpty(category))
            {
                category = DefaultCategory;
            }

            var item = _repository.Get(key, category);
            return item == null ? null : item.Value;
        }

        public T Get<T>(string key, string category = null)
        {
            var value = Get(key, category);
            var resultType = typeof(T);

            if (resultType == typeof(string))
            {
                return (T)(object)value;
            } 
            
            if (String.IsNullOrEmpty(value))
            {
                return default(T);
            }

            if (resultType.IsValueType)
            {
                return (T)Convert.ChangeType(value, resultType);
            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public IEnumerable<KeyValueSetting> GetByCategory(string category)
        {
            return _repository.Query().Where(o => o.Category == category);
        }
    }
}
