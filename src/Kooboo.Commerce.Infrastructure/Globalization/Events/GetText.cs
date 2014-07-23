using Kooboo.Commerce.Events;
using Kooboo.Commerce.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Globalization.Events
{
    public class GetText : Event
    {
        public IDictionary<EntityKey, EntityTextInfo> TextInfos { get; private set; }

        public CultureInfo Culture { get; private set; }

        public GetText(IDictionary<EntityKey, EntityTextInfo> texts, CultureInfo culture)
        {
            TextInfos = texts;
            Culture = culture;
        }

        public void SetText(EntityKey key, string property, string text)
        {
            EntityTextInfo textInfo;
            if (TextInfos.TryGetValue(key, out textInfo))
            {
                textInfo.Properties[property] = text;
            }
        }
    }
}
