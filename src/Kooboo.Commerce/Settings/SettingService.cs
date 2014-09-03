using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Newtonsoft.Json;

namespace Kooboo.Commerce.Settings
{
    [Dependency(typeof(SettingService))]
    public class SettingService
    {
        private readonly IRepository<SettingItem> _repository;

        public SettingService(ICommerceDatabase database)
        {
            _repository = database.GetRepository<SettingItem>();
        }

        public void Set<T>(T value)
        {
            Set(typeof(T).Name, value);
        }

        public void Set(string key, object value)
        {
            Require.NotNullOrEmpty(key, "key");

            var entry = _repository.Find(key);
            if (entry != null)
            {
                entry.SetValue(value);
                _repository.Database.SaveChanges();
            }
            else
            {
                entry = new SettingItem(key);
                entry.SetValue(value);
                _repository.Insert(entry);
            }
        }

        public string Get(string key)
        {
            var item = FindEntry(key);
            return item == null ? null : item.Value;
        }

        private SettingItem FindEntry(string key)
        {
            return _repository.Find(key);
        }

        public T Get<T>()
        {
            return Get<T>(typeof(T).Name);
        }

        public T Get<T>(string key)
        {
            var entry = FindEntry(key);
            if (entry == null)
            {
                return default(T);
            }

            return entry.GetValue<T>();
        }
    }
}
