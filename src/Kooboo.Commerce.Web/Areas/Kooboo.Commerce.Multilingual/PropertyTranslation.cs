using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual
{
    public class PropertyTranslation
    {
        public string Property { get; set; }

        /// <summary>
        /// Original text of current translation.
        /// </summary>
        public string OriginalText { get; set; }

        /// <summary>
        /// The translated text.
        /// </summary>
        public string TranslatedText { get; set; }

        public PropertyTranslation(string property, string originalText, string translatedText)
        {
            Property = property;
            OriginalText = originalText;
            TranslatedText = translatedText;
        }

        public PropertyTranslation Clone()
        {
            return (PropertyTranslation)base.MemberwiseClone();
        }
    }
}