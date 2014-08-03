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
        {
            Culture = culture;
            EntityKey = entityKey;
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
    }
}