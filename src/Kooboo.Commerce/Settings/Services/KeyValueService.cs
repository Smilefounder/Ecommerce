using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Settings.Services {

    [Dependency(typeof(IKeyValueService))]
    public class KeyValueService : IKeyValueService {

        private readonly IRepository<KeyValueSetting> Repository;
        public KeyValueService(IRepository<KeyValueSetting> repository) {
            Repository = repository;
        }

        public const string DefaultCategory = "__default";
        private static string EnsureCategory(string category) {
            if (string.IsNullOrEmpty(category)) {
                return DefaultCategory;
            } else {
                return category;
            }
        }

        public void Set(string key, string value) {
            this.Set(key, value, null);
        }

        public void Set(string key, string value, string category) {
            category = EnsureCategory(category);
            // remove
            var exists = Repository.Query().Where(o => o.Key == key && o.Category == category).ToList();
            foreach (var item in exists) {
                Repository.Delete(item);
            }
            // create
            Repository.Insert(new KeyValueSetting() {
                Key = key,
                Value = value,
                Category = category
            });
        }

        public string Get(string key) {
            return this.Get(key, null);
        }

        public string Get(string key, string category) {
            category = EnsureCategory(category);
            var item = Repository.Query().Where(o => o.Key == key && o.Category == category).FirstOrDefault();
            return item == null ? null : item.Value;
        }

        public IEnumerable<KeyValueSetting> GetByCategory(string category) {
            return Repository.Query().Where(o => o.Category == category);
        }
    }
}
