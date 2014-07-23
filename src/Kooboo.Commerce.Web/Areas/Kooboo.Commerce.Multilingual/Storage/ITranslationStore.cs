using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage
{
    public interface ITranslationStore
    {
        EntityTransaltion Find(CultureInfo culture, EntityKey key);

        EntityTransaltion[] Find(CultureInfo culture, params EntityKey[] keys);

        void AddOrUpdate(CultureInfo culture, EntityKey key, IDictionary<string, string> propertyTranslations);
    }
}