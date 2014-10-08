using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Extensions;
using Newtonsoft.Json;
using System.Globalization;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Multilingual.Events;

namespace Kooboo.Commerce.Multilingual.Storage.Sqlce
{
    public class SqlceTranslationStore : ITranslationStore
    {
        private ICommerceInstanceManager _instanceManager;

        public string InstanceName { get; private set; }

        public SqlceTranslationStore(string instanceName, ICommerceInstanceManager instanceManager)
        {
            Require.NotNullOrEmpty(instanceName, "instanceName");
            Require.NotNull(instanceManager, "instanceManager");

            InstanceName = instanceName;
            _instanceManager = instanceManager;
        }

        public EntityTransaltion Find(System.Globalization.CultureInfo culture, EntityKey key)
        {
            return Find(culture, new[] { key })[0];
        }

        public EntityTransaltion[] Find(System.Globalization.CultureInfo culture, params EntityKey[] keys)
        {
            var result = new EntityTransaltion[keys.Length];

            using (var db = new MultilingualDbContext(InstanceName))
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

        public int TotalTranslated(CultureInfo culture, Type entityType)
        {
            using (var db = new MultilingualDbContext(InstanceName))
            {
                return db.Translations.Where(it => it.Culture == culture.Name && it.EntityType == entityType.Name).Count();
            }
        }

        public int TotalOutOfDate(CultureInfo culture, Type entityType)
        {
            using (var db = new MultilingualDbContext(InstanceName))
            {
                return db.Translations.Where(it => it.Culture == culture.Name && it.EntityType == entityType.Name && it.IsOutOfDate).Count();
            }
        }

        public Pagination<EntityTransaltion> FindOutOfDate(CultureInfo culture, Type entityType, int pageInex, int pageSize)
        {
            using (var db = new MultilingualDbContext(InstanceName))
            {
                return db.Translations
                         .Where(t => t.Culture == culture.Name && t.EntityType == entityType.Name && t.IsOutOfDate)
                         .OrderBy(t => t.EntityKey)
                         .Paginate(pageInex, pageSize)
                         .Transform(entry =>
                         {
                             var keyType = EntityKey.GetKeyProperty(entityType).PropertyType;
                             var translation = new EntityTransaltion(culture.Name, new EntityKey(entityType, Convert.ChangeType(entry.EntityKey, keyType)))
                             {
                                 IsOutOfDate = true,
                                 PropertyTranslations = JsonConvert.DeserializeObject<List<PropertyTranslation>>(entry.Properties)
                             };

                             return translation;
                         });
            }
        }

        public void AddOrUpdate(System.Globalization.CultureInfo culture, EntityKey key, IEnumerable<PropertyTranslation> propertyTranslations)
        {
            using (var db = new MultilingualDbContext(InstanceName))
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

                Event.Raise(new TranslationUpdated(key, propertyTranslations, culture), new EventContext(GetInstance()));
            }
        }

        public void MarkOutOfDate(CultureInfo culture, EntityKey key)
        {
            using (var db = new MultilingualDbContext(InstanceName))
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
            using (var db = new MultilingualDbContext(InstanceName))
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

        private CommerceInstance GetInstance()
        {
            return _instanceManager.GetInstance(InstanceName);
        }
    }
}