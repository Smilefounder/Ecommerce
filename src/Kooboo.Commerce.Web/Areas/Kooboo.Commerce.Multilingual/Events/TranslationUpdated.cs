using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Events
{
    public class TranslationUpdated : Event
    {
        public CultureInfo Culture { get; private set; }

        public EntityKey EntityKey { get; private set; }

        public IEnumerable<PropertyTranslation> PropertyTranslations { get; private set; }

        public TranslationUpdated(EntityKey entityKey, IEnumerable<PropertyTranslation> propertyTranslations, CultureInfo culture)
        {
            EntityKey = entityKey;
            PropertyTranslations = propertyTranslations.ToList();
            Culture = culture;
        }
    }
}