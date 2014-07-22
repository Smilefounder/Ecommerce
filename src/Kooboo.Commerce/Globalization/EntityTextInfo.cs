using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Globalization
{
    public class EntityTextInfo
    {
        public EntityKey EntityKey { get; private set; }

        public TextTranslationDictionary PropertyTexts { get; private set; }

        public EntityTextInfo(EntityKey key, IDictionary<string, string> propertyTexts)
        {
            EntityKey = key;
            PropertyTexts = new TextTranslationDictionary(propertyTexts);
        }
    }
}
