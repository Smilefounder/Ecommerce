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

        /// <summary>
        /// Add or update translations for the specified entity.
        /// </summary>
        void AddOrUpdate(CultureInfo culture, EntityKey key, IEnumerable<PropertyTranslation> propertyTranslations);

        /// <summary>
        /// Mark the translations of the specified entity out of date.
        /// </summary>
        void MarkOutOfDate(CultureInfo culture, EntityKey key);
        
        void Delete(CultureInfo culture, EntityKey key);
    }
}