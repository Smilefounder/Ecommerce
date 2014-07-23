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

        public TextDictionary Properties { get; private set; }

        public EntityTransaltion(string culture, EntityKey key)
            : this(culture, key, null)
        {
        }

        public EntityTransaltion(string culture, EntityKey key, IDictionary<string, string> properties)
        {
            Culture = culture;
            EntityKey = key;
            Properties = properties == null ? new TextDictionary() : new TextDictionary(properties);
        }
    }
}