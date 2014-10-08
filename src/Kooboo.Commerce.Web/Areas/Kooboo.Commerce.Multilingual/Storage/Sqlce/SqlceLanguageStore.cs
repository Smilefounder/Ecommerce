using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Multilingual.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage.Sqlce
{
    public class SqlceLanguageStore : ILanguageStore
    {
        private ICommerceInstanceManager _instanceManager;

        public string InstanceName { get; private set; }

        public SqlceLanguageStore(string instanceName, ICommerceInstanceManager instanceManager)
        {
            Require.NotNull(instanceName, "instanceName");
            Require.NotNull(instanceManager, "instanceManager");

            InstanceName = instanceName;
            _instanceManager = instanceManager;
        }

        public IEnumerable<Language> All()
        {
            using (var db = new MultilingualDbContext(InstanceName))
            {
                return db.Languages.ToList();
            }
        }

        public bool Exists(string name)
        {
            using (var db = new MultilingualDbContext(InstanceName))
            {
                return db.Languages.Any(l => l.Name == name);
            }
        }

        public Language Find(string name)
        {
            using (var db = new MultilingualDbContext(InstanceName))
            {
                return db.Languages.Find(name);
            }
        }

        public void Add(Language language)
        {
            using (var db = new MultilingualDbContext(InstanceName))
            {
                db.Languages.Add(language);
                db.SaveChanges();

                Event.Raise(new LanguageAdded { Name = language.Name }, new EventContext(GetInstance()));
            }
        }

        public void Update(Language language)
        {
            using (var db = new MultilingualDbContext(InstanceName))
            {
                var existing = db.Languages.Find(language.Name);
                existing.DisplayName = language.DisplayName;
                db.SaveChanges();
            }
        }

        public void Delete(string name)
        {
            using (var db = new MultilingualDbContext(InstanceName))
            {
                var lang = db.Languages.Find(name);
                db.Languages.Remove(lang);
                db.SaveChanges();

                Event.Raise(new LanguageDeleted { Name = lang.Name }, new EventContext(GetInstance()));
            }
        }

        private CommerceInstance GetInstance()
        {
            return _instanceManager.GetInstance(InstanceName);
        }
    }
}