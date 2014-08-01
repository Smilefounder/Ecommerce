using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Extensions;
using Newtonsoft.Json;
using Kooboo.CMS.Common.Runtime.Dependency;
using System.Globalization;

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
                    var entry = db.Translations.Find(GetUnderlyingEntityKey(culture.Name, key));

                    if (entry != null)
                    {
                        var translation = new EntityTransaltion(culture.Name, new EntityKey(key.EntityType, key.Value))
                        {
                            IsOutOfDate = entry.IsOutOfDate,
                            PropertyTranslations = JsonConvert.DeserializeObject<List<PropertyTranslation>>(entry.Properties)
                        };

                        result[i] = translation;
                    }
                }
            }

            return result;
        }

        public void AddOrUpdate(System.Globalization.CultureInfo culture, EntityKey key, IEnumerable<PropertyTranslation> propertyTranslations)
        {
            using (var db = new MultilingualDbContext(CurrentInstance().Name))
            {
                var entry = db.Translations.Find(GetUnderlyingEntityKey(culture.Name, key));
                if (entry == null)
                {
                    entry = new EntityTranslationDbEntry
                    {
                        Culture = culture.Name,
                        EntityType = key.EntityType.Name,
                        EntityKey = key.Value.ToString()
                    };

                    entry.Properties = JsonConvert.SerializeObject(propertyTranslations);
                    db.Translations.Add(entry);
                }
                else
                {
                    entry.Properties = JsonConvert.SerializeObject(propertyTranslations);
                    entry.IsOutOfDate = false;
                }

                db.SaveChanges();
            }
        }

        public void MarkOutOfDate(CultureInfo culture, EntityKey key)
        {
            using (var db = new MultilingualDbContext(CurrentInstance().Name))
            {
                var entry = db.Translations.Find(GetUnderlyingEntityKey(culture.Name, key));
                if (entry != null)
                {
                    entry.IsOutOfDate = true;
                    db.SaveChanges();
                }
            }
        }

        public void Delete(CultureInfo culture, EntityKey key)
        {
            using (var db = new MultilingualDbContext(CurrentInstance().Name))
            {
                var entry = db.Translations.Find(GetUnderlyingEntityKey(culture.Name, key));
                if (entry != null)
                {
                    db.Translations.Remove(entry);
                    db.SaveChanges();
                }
            }
        }

        private object[] GetUnderlyingEntityKey(string culture, EntityKey key)
        {
            return new[] { culture, key.EntityType.Name, key.Value.ToString() };
        }
    }
}