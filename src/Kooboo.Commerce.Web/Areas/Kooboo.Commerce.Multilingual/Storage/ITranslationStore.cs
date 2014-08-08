using Kooboo.Commerce.Data;
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

        int TotalTranslated(CultureInfo culture, Type entityType);

        int TotalOutOfDate(CultureInfo culture, Type entityType);

        Pagination<EntityTransaltion> FindOutOfDate(CultureInfo culture, Type entityType, int pageIndex, int pageSize); 

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