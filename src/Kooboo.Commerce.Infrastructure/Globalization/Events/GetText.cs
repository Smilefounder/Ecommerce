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
        public IDictionary<EntityKey, TextDictionary> Texts { get; private set; }

        public CultureInfo Culture { get; private set; }

        public GetText(IDictionary<EntityKey, TextDictionary> texts, CultureInfo culture)
        {
            Texts = texts;
            Culture = culture;
        }
    }
}
