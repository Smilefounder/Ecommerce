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

            var entry = _repository.Get(key, category);
            if (entry != null)
            {
                entry.SetValue(value);
                _repository.Database.SaveChanges();
            }
            else
            {
                entry = new KeyValueSetting(key, category);
                entry.SetValue(value);
                _repository.Insert(entry);
            }
        }

        public string Get(string key, string category = null)
        {
            var item = FindEntry(key, category);
            return item == null ? null : item.Value;
        }

        private KeyValueSetting FindEntry(string key, string category = null)
        {
            if (String.IsNullOrEmpty(category))
            {
                category = DefaultCategory;
            }

            return _repository.Get(key, category);
        }

        public T Get<T>(string key, string category = null)
        {
            var entry = FindEntry(key, category);
            if (entry == null)
            {
                return default(T);
            }

            return entry.GetValue<T>();
        }

        public IEnumerable<KeyValueSetting> GetByCategory(string category)
        {
            return _repository.Query().Where(o => o.Category == category);
        }
    }
}
