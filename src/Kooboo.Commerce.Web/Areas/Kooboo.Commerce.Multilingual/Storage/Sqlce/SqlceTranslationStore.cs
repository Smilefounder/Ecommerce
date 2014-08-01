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
                    // If it's updating, sync OriginalText with UpdatedOriginalText if OriginalText is not set
                    var props = propertyTranslations.Select(p => p.Clone()).ToList();
                    foreach (var prop in props)
                    {
                        if (prop.OriginalText == null)
                        {
                            prop.OriginalText = prop.UpdatedOrignalText;
                        }
                    }

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
                entry.IsOutOfDate = true;
                db.SaveChanges();
            }
        }

        public bool MarkOutOfDate(CultureInfo culture, EntityKey key, IDictionary<string, string> propertyUpdates)
        {
            using (var db = new MultilingualDbContext(CurrentInstance().Name))
            {
                var entry = db.Translations.Find(GetUnderlyingEntityKey(culture.Name, key));
                // If no entry is found, it means that this entity is never translated, and we simply ingore it
                if (entry != null)
                {
                    var updated = false;
                    var properties = JsonConvert.DeserializeObject<List<PropertyTranslation>>(entry.Properties);

                    foreach (var each in propertyUpdates)
                    {
                        var prop = properties.Find(p => p.Property == each.Key);
                        if (prop != null)
                        {
                            if (prop.OriginalText == null || !prop.OriginalText.Equals(each.Value))
                            {
                                prop.UpdatedOrignalText = each.Value;
                                updated = true;
                            }
                        }
                    }

                    if (updated)
                    {
                        entry.IsOutOfDate = true;
                        entry.Properties = JsonConvert.SerializeObject(properties);
                        db.SaveChanges();
                    }

                    return updated;
                }

                return false;
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