using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual
{
    public class EntityTransaltion
    {
        public string Culture { get; private set; }

        public EntityKey EntityKey { get; private set; }

        public bool IsOutOfDate { get; set; }

        public IList<PropertyTranslation> PropertyTranslations { get; set; }

        public EntityTransaltion(string culture, EntityKey key)
        {
            Culture = culture;
            EntityKey = key;
            PropertyTranslations = new List<PropertyTranslation>();
        }

        public string GetTranslatedText(string property)
        {
            var translation = PropertyTranslations.FirstOrDefault(t => t.Property == property);
            return translation == null ? null : translation.TranslatedText;
        }
    }
}