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
        public string Instance { get; private set; }

        public SqlceLanguageStore(string instance)
        {
            Require.NotNullOrEmpty(instance, "instance");
            Instance = instance;
        }

        public IEnumerable<Language> All()
        {
            using (var db = new MultilingualDbContext(Instance))
            {
                return db.Languages.ToList();
            }
        }

        public bool Exists(string name)
        {
            using (var db = new MultilingualDbContext(Instance))
            {
                return db.Languages.Any(l => l.Name == name);
            }
        }

        public Language Find(string name)
        {
            using (var db = new MultilingualDbContext(Instance))
            {
                return db.Languages.Find(name);
            }
        }

        public void Add(Language language)
        {
            using (var db = new MultilingualDbContext(Instance))
            {
                db.Languages.Add(language);
                db.SaveChanges();

                Event.Raise(new LanguageAdded(language.Name));
            }
        }

        public void Update(Language language)
        {
            using (var db = new MultilingualDbContext(Instance))
            {
                var existing = db.Languages.Find(language.Name);
                existing.DisplayName = language.DisplayName;
                db.SaveChanges();
            }
        }

        public void Delete(string name)
        {
            using (var db = new MultilingualDbContext(Instance))
            {
                var lang = db.Languages.Find(name);
                db.Languages.Remove(lang);
                db.SaveChanges();

                Event.Raise(new LanguageDeleted(lang.Name));
            }
        }
    }
}