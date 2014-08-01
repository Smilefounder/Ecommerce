using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Extensions;
using Newtonsoft.Json;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.Commerce.Multilingual.Storage.Sqlce
{
    [Dependency(typeof(ITranslationStore))]
    public class SqlceTranslationStore : ITranslationStore
    {
        public Func<CommerceInstance> CurrentInstance = () => CommerceInstance.Current;

        public EntityTransaltion Find(System.Globalization.CultureInfo culture, EntityKey key)
        {
            return Find(culture, new [] { key })[0];
        }

        public EntityTransaltion[] Find(System.Globalization.CultureInfo culture, params EntityKey[] keys)
        {
            var result = new EntityTransaltion[keys.Length];

            using (var db = new MultilingualDbContext(CurrentInstance().Name))
            {
                for (var i = 0; i < keys.Length; i++)
                {
                    var key = keys[i];
                    var entry = db.Translations.Find(new[] { culture.Name, key.EntityType.Name, key.Value.ToString() });

                    if (entry != null)
                    {
                        var props = JsonConvert.DeserializeObject<IDictionary<string, string>>(entry.Properties);
                        var translation = new EntityTransaltion(culture.Name, new EntityKey(key.EntityType, key.Value), props);
                        result[i] = translation;
                    }
                }
            }

            return result;
        }

        public void AddOrUpdate(System.Globalization.CultureInfo culture, EntityKey key, IDictionary<string, string> propertyTranslations)
        {
            using (var db = new MultilingualDbContext(CurrentInstance().Name))
            {
                var entry = db.Translations.Find(new[] { culture.Name, key.EntityType.Name, key.Value.ToString() });
                if (entry == null)
                {
                    entry = new EntityTranslationDbEntry
                    {
                        Culture = culture.Name,
                        EntityType = key.EntityType.Name,
                        EntityKey = key.Value.ToString()
                    };

                    db.Translations.Add(entry);
                }

                entry.Properties = JsonConvert.SerializeObject(propertyTranslations);

                db.SaveChanges();
            }
        }
    }
}