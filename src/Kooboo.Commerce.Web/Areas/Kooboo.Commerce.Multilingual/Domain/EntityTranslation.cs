using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Domain
{
    public class EntityTransaltion
    {
        public string Culture { get; private set; }

        public EntityKey EntityKey { get; private set; }

        public TextTranslationDictionary PropertyTranslations { get; private set; }

        public EntityTransaltion(string culture, EntityKey key)
            : this(culture, key, null)
        {
        }

        public EntityTransaltion(string culture, EntityKey key, IDictionary<string, string> propertyTranslations)
        {
            Culture = culture;
            EntityKey = key;
            PropertyTranslations = propertyTranslations == null ? new TextTranslationDictionary() : new TextTranslationDictionary(propertyTranslations);
        }
    }
}