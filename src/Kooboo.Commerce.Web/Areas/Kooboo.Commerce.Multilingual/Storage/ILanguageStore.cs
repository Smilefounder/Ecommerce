using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage
{
    public interface ILanguageStore
    {
        IEnumerable<Language> All();

        bool Exists(string name);

        Language Find(string name);

        void Add(Language language);

        void Update(Language language);

        void Delete(string name);
    }
}