using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual
{
    public class EntityTransaltion
    {
        public string Culture { get; set; }

        public EntityKey EntityKey { get; set; }

        public bool IsOutOfDate { get; set; }

        public IList<PropertyTranslation> PropertyTranslations { get; set; }

        public EntityTransaltion(string culture, EntityKey entityKey)
            : this(culture, entityKey, null)
        {
        }

        public EntityTransaltion(string culture, EntityKey entityKey, IEnumerable<PropertyTranslation> propertyTranslations)
        {
            Culture = culture;
            EntityKey = entityKey;
            PropertyTranslations = propertyTranslations == null ? new List<PropertyTranslation>() : propertyTranslations.ToList();
        }

        public string GetTranslatedText(string property)
        {
            var translation = PropertyTranslations.FirstOrDefault(t => t.Property == property);
            return translation == null ? null : translation.TranslatedText;
        }

        public string GetOriginalText(string property)
        {
            var translation = PropertyTranslations.FirstOrDefault(t => t.Property == property);
            return translation == null ? null : translation.OriginalText;
        }

        public EntityTransaltion Clone()
        {
            var clone = new EntityTransaltion(Culture, EntityKey)
            {
                IsOutOfDate = IsOutOfDate,
                PropertyTranslations = PropertyTranslations.Select(t => t.Clone()).ToList()
            };

            return clone;
        }
    }
}