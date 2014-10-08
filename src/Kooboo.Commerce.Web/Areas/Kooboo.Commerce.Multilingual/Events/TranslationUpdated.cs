using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Events
{
    public class TranslationUpdated : IEvent
    {
        public CultureInfo Culture { get; set; }

        public EntityKey EntityKey { get; set; }

        public IEnumerable<PropertyTranslation> PropertyTranslations { get; set; }

        public TranslationUpdated(EntityKey entityKey, IEnumerable<PropertyTranslation> propertyTranslations, CultureInfo culture)
        {
            EntityKey = entityKey;
            PropertyTranslations = propertyTranslations.ToList();
            Culture = culture;
        }
    }
}