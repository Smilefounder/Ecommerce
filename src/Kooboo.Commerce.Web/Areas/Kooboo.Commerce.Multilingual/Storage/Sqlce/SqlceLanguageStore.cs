using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Multilingual.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage.Sqlce
{
    [Dependency(typeof(ILanguageStore))]
    public class SqlceLanguageStore : ILanguageStore
    {
        public Func<CommerceInstance> CurrentInstance = () => CommerceInstance.Current;

        public IEnumerable<Language> All()
        {
            using (var db = new MultilingualDbContext(CurrentInstance().Name))
            {
                return db.Languages.ToList();
            }
        }

        public Language Find(string name)
        {
            using (var db = new MultilingualDbContext(CurrentInstance().Name))
            {
                return db.Languages.Find(name);
            }
        }

        public void Add(Language language)
        {
            using (var db = new MultilingualDbContext(CurrentInstance().Name))
            {
                db.Languages.Add(language);
                db.SaveChanges();

                Event.Raise(new LanguageAdded(language.Name));
            }
        }

        public void Update(Language language)
        {
            using (var db = new MultilingualDbContext(CurrentInstance().Name))
            {
                var existing = db.Languages.Find(language.Name);
                existing.DisplayName = language.DisplayName;
                db.SaveChanges();
            }
        }

        public void Delete(string name)
        {
            using (var db = new MultilingualDbContext(CurrentInstance().Name))
            {
                var lang = db.Languages.Find(name);
                db.Languages.Remove(lang);
                db.SaveChanges();

                Event.Raise(new LanguageDeleted(lang.Name));
            }
        }
    }
}